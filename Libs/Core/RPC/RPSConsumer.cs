using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace Core.RPC;

/// <summary>
/// RPSConsumer - класс, ответственный за асинхронное потребление удаленных вызовов процедур (RPC). 
/// Он устанавливает соединение с сервером RabbitMQ и создает очередь для приема запросов RPC. 
/// Также предоставляет метод для выполнения RPC-вызовов и асинхронного получения ответов.
/// </summary>
public class RPSConsumer : IDisposable
{
    private const string QueueName = "rpc_queue";

    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new();

    /// <summary>
    /// Конструктор класса RPSConsumer. Инициализирует соединение и канал с сервером RabbitMQ, 
    /// объявляет очередь для приема запросов RPC и настраивает обработчик для асинхронной обработки входящих сообщений.
    /// </summary>
    public RPSConsumer()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _queueName = _channel.QueueDeclare().QueueName;

        var consumer = new EventingBasicConsumer(model: _channel);
        consumer.Received += (model, ea) =>
        {
            if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;

            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);

            tcs.TrySetResult(response);
        };

        _channel.BasicConsume(
            consumer: consumer,
            queue: _queueName,
            autoAck: true);
    }

    /// <summary>
    /// Выполняет асинхронный RPC-вызов с указанным сообщением. Возвращает задачу, представляющую ответ от сервера RPC.
    /// </summary>
    /// <param name="message">Сообщение, которое будет отправлено в рамках вызова RPC.</param>
    /// <param name="cancellationToken">(Опционально) CancellationToken, который можно использовать для отмены вызова RPC.</param>
    /// <returns>Задача Task<string>, представляющая ответ от сервера RPC.</returns>
    public Task<string> CallAsync(string message, CancellationToken cancellationToken = default)
    {
        var props = _channel.CreateBasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = _queueName;

        var messageBytes = Encoding.UTF8.GetBytes(message);
        var tcs = new TaskCompletionSource<string>();
        callbackMapper.TryAdd(correlationId, tcs);

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: QueueName,
            basicProperties: props,
            body: messageBytes);

        cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out _));
        return tcs.Task;
    }

    public void Dispose()
    {
        _connection.Close();
    }
}
