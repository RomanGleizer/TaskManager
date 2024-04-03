using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace ConnectionLib.ConnectionServices.BackgroundConnectionServices;

public class RabbitMqConnectionPoolPolicy(ConnectionFactory connectionFactory) : IPooledObjectPolicy<IConnection>
{
    public IConnection Create()
    {
        return connectionFactory.CreateConnection();
    }

    public bool Return(IConnection connection)
    {
        return true;
    }
}