using ConnectionLib.ConnectionServices.DtoModels.AddProjectToListOfUserProjects;
using Logic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConnectionLib.ConnectionServices.BackgroundConnectionServices;

public class RabbitMQBackgroundUserConnectionService<TService> : BackgroundService
    where TService : IUserService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RabbitMQBackgroundUserConnectionService(IServiceScopeFactory serviceScopeFactory)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        _queueName = "UserConnectionServiceQueue";
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _serviceScopeFactory = serviceScopeFactory;

        _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(model: _channel);
        consumer.Received += (model, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            _channel.BasicAck(ea.DeliveryTag, false);

            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            var addNewTaskDesirializeData = JsonConvert.DeserializeObject<AddProjectToListOfUserProjectsResponse>(message)
                ?? throw new Exception("Произошла ошибка при десериализации типа данных AddProjectToListOfUserProjectsResponse");

            using var scope = _serviceScopeFactory.CreateScope();

            var userService = scope.ServiceProvider.GetRequiredService<TService>()
                ?? throw new Exception("Произошла ошибка при получении UserService");

            var result = userService.AddNewProject(addNewTaskDesirializeData.ProjectId, addNewTaskDesirializeData.MemberId).Result;
        };

        _channel.BasicConsume(
            consumer: consumer,
            queue: _queueName,
            autoAck: true);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();

        base.Dispose();
    }
}
