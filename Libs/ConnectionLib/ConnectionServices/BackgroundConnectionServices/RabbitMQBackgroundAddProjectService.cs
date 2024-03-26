using ConnectionLib.ConnectionServices.DtoModels.AddProjectToListOfUserProjects;
using Core.Dal.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConnectionLib.ConnectionServices.BackgroundConnectionServices;

/// <summary>
/// Фоновый сервис для управления подключениями пользователей через RabbitMQ
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса <see cref="RabbitMQBackgroundAddProjectService"/>
/// </remarks>
/// <param name="serviceProvider">Поставщик служб</param>
public class RabbitMQBackgroundAddProjectService(IServiceProvider serviceProvider, ObjectPool<IConnection> connectionPool) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly string _queueName = "UserConnectionServiceQueue";
    private readonly ObjectPool<IConnection> _connectionPool = connectionPool;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var connection = _connectionPool.Get();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                channel.BasicAck(ea.DeliveryTag, false);

                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                var addNewTaskDeserializedData = JsonConvert.DeserializeObject<AddProjectToListOfUserProjectsRequest>(message)
                    ?? throw new Exception($"Ошибка при десериализации {typeof(AddProjectToListOfUserProjectsRequest)}");

                using var scope = _serviceProvider.CreateScope();

                var addProjectIdToProjectIdList = scope.ServiceProvider.GetRequiredService<IAddProjectIdToUserProjectIdList>();
                await addProjectIdToProjectIdList.AddProjectIdToProjectIdListAsync(addNewTaskDeserializedData.ProjectId, addNewTaskDeserializedData.MemberId);
            };

            channel.BasicConsume(consumer: consumer, queue: _queueName, autoAck: true);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
