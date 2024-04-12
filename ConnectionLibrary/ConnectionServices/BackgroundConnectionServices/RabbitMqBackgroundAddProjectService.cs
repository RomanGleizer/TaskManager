using System.Text;
using ConnectionLibrary.ConnectionServices.DtoModels.AddProjectToListOfUserProjects;
using Core.Dal.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConnectionLibrary.ConnectionServices.BackgroundConnectionServices;

/// <summary>
///     Фоновый сервис для управления подключениями пользователей через RabbitMQ
/// </summary>
/// <remarks>
///     Инициализирует новый экземпляр класса <see cref="RabbitMqBackgroundAddProjectService" />
/// </remarks>
/// <param name="serviceProvider">Поставщик служб</param>
public class RabbitMqBackgroundAddProjectService(
    IServiceProvider serviceProvider,
    ObjectPool<IConnection> connectionPool)
    : BackgroundService
{
    private readonly string _queueName = "UserConnectionServiceQueue";

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

                var addNewTaskDeserializedData =
                    JsonConvert.DeserializeObject<AddProjectToListOfUserProjectsRequest>(message)
                    ?? throw new Exception(
                        $"Ошибка при десериализации {typeof(AddProjectToListOfUserProjectsRequest)}");

                using var scope = serviceProvider.CreateScope();

                var addProjectIdToProjectIdList =
                    scope.ServiceProvider.GetRequiredService<IAddProjectIdToUserProjectIdList>();

                await addProjectIdToProjectIdList.AddProjectIdToProjectIdListAsync(
                    addNewTaskDeserializedData.ProjectId,
                    addNewTaskDeserializedData.MemberId);
            };

            channel.BasicConsume(consumer: consumer, queue: _queueName, autoAck: true);
            await Task.Delay(1000, stoppingToken);
        }
    }
}