using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;


namespace ConnectionLib.ConnectionServices.BackgroundConnectionServices;

public class RabbitMQConnectionPooledObjectPolicy : IPooledObjectPolicy<IConnection>
{
    private readonly ConnectionFactory _connectionFactory;

    public RabbitMQConnectionPooledObjectPolicy(string hostName)
    {
        _connectionFactory = new ConnectionFactory { HostName = hostName };
    }

    public IConnection Create()
    {
        return _connectionFactory.CreateConnection();
    }

    public bool Return(IConnection obj)
    {
        return true;
    }
}