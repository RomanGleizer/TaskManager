using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Core.RPC;

public class RPSConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;

    public RPSConsumer()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        _queueName = "IdentityServiceQueue";
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(model: _channel);
        consumer.Received += (model, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            _channel.BasicAck(ea.DeliveryTag, false);

            Console.WriteLine(content);
        };

        _channel.BasicConsume(
            consumer: consumer,
            queue: _queueName,
            autoAck: true);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(model: _channel);
        consumer.Received += (model, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            _channel.BasicAck(ea.DeliveryTag, false);
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
