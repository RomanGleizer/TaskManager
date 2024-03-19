using ConnectionLib.ConnectionServices.DtoModels.ProjectById;
using Core.Dal.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConnectionLib.ConnectionServices.BackgroundConnectionServices;

/// <summary>
/// Фоновый сервис для получения информации о проекте через RabbitMQ
/// </summary>
/// <typeparam name="TModel">Тип модели проекта, реализующий интерфейс <see cref="IBaseEntity{TKey}"/> с ключом типа int</typeparam>
/// <remarks>
/// Инициализирует новый экземпляр класса <see cref="RabbitMQBackgroundGetProjectService{TModel}"/>
/// </remarks>
/// <param name="serviceProvider">Поставщик служб</param>
public class RabbitMQBackgroundGetProjectService<TModel>(IServiceProvider serviceProvider) : BackgroundService
    where TModel : IBaseEntity<int>
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly string _queueName = "GetProjectQueue";

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var getProjectById = scope.ServiceProvider.GetRequiredService<IGetProjectById<TModel>>();
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        // Объявление очереди
        channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(model: channel);
        consumer.Received += async (model, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            channel.BasicAck(ea.DeliveryTag, false);

            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            var getProjectDesirializedData = JsonConvert.DeserializeObject<IsProjectExistsRequest>(message)
                ?? throw new Exception($"Ошибка при десериализации {typeof(IsProjectExistsRequest)}");

            // Получение информации о проекте по его идентификатору
            await getProjectById.GetById(getProjectDesirializedData.ProjectId);
        };

        // Потребление сообщений из очереди
        channel.BasicConsume(
            consumer: consumer,
            queue: _queueName,
            autoAck: true);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
