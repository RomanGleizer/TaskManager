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
public class RabbitMQBackgroundAddProjectService(IServiceScopeFactory serviceScopeFactory, ObjectPool<IConnection> connectionPool) : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly string _queueName = "UserConnectionServiceQueue";
    private readonly ObjectPool<IConnection> _connectionPool = connectionPool;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var addProjectIdToProjectIdList = scope.ServiceProvider.GetRequiredService<IAddProjectIdToUserProjectIdList>();
        var connection = _connectionPool.Get();
        var channel = connection.CreateModel();

        // Объявление очереди
        channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(model: channel);
        consumer.Received += async (model, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            channel.BasicAck(ea.DeliveryTag, false);

            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            var addNewTaskDeserializedData = JsonConvert.DeserializeObject<AddProjectToListOfUserProjectsRequest>(message)
                ?? throw new Exception($"Ошибка при десериализации {typeof(AddProjectToListOfUserProjectsRequest)}");

            // Добавление идентификатора проекта в список проектов пользователя
            var result = await addProjectIdToProjectIdList.AddProjectIdToProjectIdListAsync(addNewTaskDeserializedData.ProjectId, addNewTaskDeserializedData.MemberId);
        };

        // Потребление сообщений из очереди
        channel.BasicConsume(
            consumer: consumer,
            queue: _queueName,
            autoAck: true);

        await Task.CompletedTask;
        _connectionPool.Return(connection);
    }
}