using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConnectionLib.ConnectionServices.BackgroundConnectionServices;

public class RabbitMQBackgroundTaskConnectionService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RabbitMQBackgroundTaskConnectionService(IServiceScopeFactory serviceScopeFactory)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        _queueName = "TaskConnectionServiceQueue";
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _serviceScopeFactory = serviceScopeFactory;
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

            // Логика добавления задачи
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