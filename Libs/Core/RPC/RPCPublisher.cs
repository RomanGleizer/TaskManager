using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Core.RPC;

public class RPCPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RPCPublisher(string queueName, object message, string hostName = "localhost", string exchange = "")
    {
        var factory = new ConnectionFactory { HostName = hostName };

        using (_connection = factory.CreateConnection())
            using (_channel = _connection.CreateModel())
            {
                _channel.QueueDeclare(
                    queue: queueName,
                    exclusive: false,
                    durable: true,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                _channel.BasicPublish(
                    exchange: exchange,
                    routingKey: queueName,
                    basicProperties: null,
                    body: body);
            }
    }
}
