using System.Text;
using ConnectionLibrary.ConnectionServices.DtoModels.ProjectById;
using Core.Dal.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConnectionLibrary.ConnectionServices.BackgroundConnectionServices;

/// <summary>
///     Фоновый сервис для получения информации о проекте через RabbitMQ
/// </summary>
/// <typeparam name="TModel">Тип модели проекта, реализующий интерфейс <see cref="IBaseEntity{TKey}" /> с ключом типа int</typeparam>
/// <remarks>
///     Инициализирует новый экземпляр класса <see cref="RabbitMqBackgroundGetProjectService{TModel}" />
/// </remarks>
/// <param name="serviceProvider">Поставщик служб</param>
public class RabbitMqBackgroundGetProjectService<TModel>(
    IServiceProvider serviceProvider,
    ObjectPool<IConnection> connectionPool) : BackgroundService
    where TModel : IBaseEntity<Guid>
{
    private readonly string _queueName = "GetProjectQueue";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var connection = connectionPool.Get();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(_queueName, true, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                channel.BasicAck(ea.DeliveryTag, false);

                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                var getProjectDeserializedData = JsonConvert.DeserializeObject<IsProjectExistsRequest>(message)
                                                 ?? throw new Exception(
                                                     $"Ошибка при десериализации {typeof(IsProjectExistsRequest)}");

                using var scope = serviceProvider.CreateScope();

                var getProjectById = scope.ServiceProvider.GetRequiredService<IGetProjectById<TModel>>();
                await getProjectById.GetById(getProjectDeserializedData.ProjectId);
            };

            channel.BasicConsume(consumer: consumer, queue: _queueName, autoAck: true);
            await Task.Delay(1000, stoppingToken);
        }
    }
}