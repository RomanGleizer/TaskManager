using ConnectionLib.ConnectionServices.DtoModels.AddProjectToListOfUserProjects;
using Core.Dal.Base;
using Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConnectionLib.ConnectionServices.BackgroundConnectionServices;

public class RabbitMQBackgroundUserConnectionService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly string _queueName = "UserConnectionServiceQueue";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserDal>>();
        var addProjectIdToProjectIdList = scope.ServiceProvider.GetRequiredService<IAddProjectIdToProjectIdList>();
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(model: channel);
        consumer.Received += async (model, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            channel.BasicAck(ea.DeliveryTag, false);

            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            var addNewTaskDesirializeData = JsonConvert.DeserializeObject<AddProjectToListOfUserProjectsRequest>(message)
                ?? throw new Exception($"Ошибка при десериализации {typeof(AddProjectToListOfUserProjectsRequest)}");

            var result = await addProjectIdToProjectIdList.AddProjectIdToProjectIdListAsync(addNewTaskDesirializeData.ProjectId, addNewTaskDesirializeData.MemberId);
        };

        channel.BasicConsume(
            consumer: consumer,
            queue: _queueName,
            autoAck: true);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
