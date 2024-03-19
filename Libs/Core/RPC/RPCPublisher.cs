using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Core.RPC;

/// <summary>
/// Представляет издателя (publisher) для сообщений удаленного вызова процедур
/// </summary>
/// <typeparam name="T">Тип данных, которые будут опубликованы</typeparam>
public class RPCPublisher<T> : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly T _data;
    private readonly string _exchange;

    /// <summary>
    /// Инициализирует новый экземпляр класса RPCPublisher с указанным именем очереди, данными, именем хоста и обмена
    /// </summary>
    /// <param name="queueName">Имя очереди</param>
    /// <param name="data">Данные, которые будут опубликованы</param>
    /// <param name="hostName">Имя хоста брокера сообщений. По умолчанию "localhost"</param>
    /// <param name="exchange">Имя обмена. По умолчанию пустая строка</param>
    public RPCPublisher(string queueName, T data, string hostName = "localhost", string exchange = "")
    {
        _queueName = queueName;
        _data = data;
        _exchange = exchange;

        var factory = new ConnectionFactory { HostName = hostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    /// <summary>
    /// Публикует сообщение в очередь сообщений
    /// </summary>
    /// <returns>True, если сообщение успешно опубликовано; в противном случае false</returns>
    public async Task<bool> PublishAsync()
    {
        try
        {
            if (_exchange != string.Empty)
                _channel.ExchangeDeclare(_queueName, _exchange);

            _channel.QueueDeclare(
                queue: _queueName,
                exclusive: false,
                durable: true,
                autoDelete: false,
                arguments: null);

            var serializedData = JsonConvert.SerializeObject(_data);
            var body = Encoding.UTF8.GetBytes(serializedData);
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            await Task.Run(() => _channel.BasicPublish(
                exchange: _exchange,
                routingKey: _queueName,
                basicProperties: properties,
                body: body));

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Произошла ошибка при работе издателя RPC", ex);
        }
    }

    /// <summary>
    /// Освобождает ресурсы, связанные с соединением и каналом
    /// </summary>
    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
