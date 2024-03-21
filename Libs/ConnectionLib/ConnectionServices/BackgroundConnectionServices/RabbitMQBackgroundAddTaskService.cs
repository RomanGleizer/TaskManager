using ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;
using Core.Dal.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConnectionLib.ConnectionServices.BackgroundConnectionService;

/// <summary>
/// Фоновый сервис для прослушивания RabbitMQ и добавления новых задач в списки проектов
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса <see cref="RabbitMQBackgroundAddTaskService"/>
/// </remarks>
/// <param name="serviceProvider">Поставщик служб</param>
public class RabbitMQBackgroundAddTaskService(IServiceScopeFactory serviceProvider, ObjectPool<IConnection> connectionPool) : BackgroundService
{
    private readonly IServiceScopeFactory _serviceProvider = serviceProvider;
    private readonly string _queueName = "AddTaskQueue";
    private readonly ObjectPool<IConnection> _connectionPool = connectionPool;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var addTaskIdToProjectIdList = scope.ServiceProvider.GetRequiredService<IAddTaskIdToProjectTaskIdList>();

        using var connection = _connectionPool.Get();
        using var channel = connection.CreateModel();

        // Объявление очереди
        channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(model: channel);
        consumer.Received += async (model, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            channel.BasicAck(ea.DeliveryTag, false);

            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            var addNewTaskDeserializedData = JsonConvert.DeserializeObject<AddTaskIdInProjectTaskIdsRequest>(message)
                ?? throw new Exception($"Ошибка при десериализации {typeof(AddTaskIdInProjectTaskIdsRequest)}");

            // Добавление нового идентификатора задачи в список идентификаторов проекта
            await addTaskIdToProjectIdList.AddNewTaskIdInProjectIdList(addNewTaskDeserializedData.ProjectId, addNewTaskDeserializedData.TaskId);
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