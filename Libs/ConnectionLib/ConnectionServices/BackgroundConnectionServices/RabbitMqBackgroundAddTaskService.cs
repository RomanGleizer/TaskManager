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
/// Инициализирует новый экземпляр класса <see cref="RabbitMqBackgroundAddTaskService"/>
/// </remarks>
/// <param name="serviceProvider">Поставщик служб</param>
public class RabbitMqBackgroundAddTaskService(
    IServiceProvider serviceProvider, 
    ObjectPool<IConnection> connectionPool) 
    : BackgroundService
{
    private readonly string _queueName = "AddTaskQueue";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var connection = connectionPool.Get();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                channel.BasicAck(ea.DeliveryTag, false);

                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                var addNewTaskDeserializedData = JsonConvert.DeserializeObject<AddTaskIdInProjectTaskIdsRequest>(message)
                    ?? throw new Exception($"Ошибка при десериализации {typeof(AddTaskIdInProjectTaskIdsRequest)}");

                using var scope = serviceProvider.CreateScope();

                var addTaskIdToProjectIdList = scope.ServiceProvider.GetRequiredService<IAddTaskIdToProjectTaskIdList>();
                
                await addTaskIdToProjectIdList.AddNewTaskIdInProjectIdList(
                    addNewTaskDeserializedData.ProjectId, 
                    addNewTaskDeserializedData.TaskId);
            };

            channel.BasicConsume(consumer: consumer, queue: _queueName, autoAck: true);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
