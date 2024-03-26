using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace ConnectionLib.ConnectionServices.BackgroundConnectionServices;

public class RabbitMQConnectionPoolPolicy(ConnectionFactory connectionFactory) : IPooledObjectPolicy<IConnection>
{
    private readonly ConnectionFactory _connectionFactory = connectionFactory;

    public IConnection Create()
    {
        return _connectionFactory.CreateConnection();
    }

    public bool Return(IConnection connection)
    {
        return true;
    }
}