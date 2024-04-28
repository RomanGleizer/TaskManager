using SemaphoreSynchronizationPrimitiveLibrary.Interfaces;
using StackExchange.Redis;

namespace SemaphoreSynchronizationPrimitiveLibrary.Semaphores;

/// <summary>
/// Реализация распределенного семафора с использованием Redis
/// </summary>
public class RedisDistributedSemaphore : IDistributedSemaphore
{
    private readonly TimeSpan _timeout;
    private readonly IConnectionMultiplexer _redis;

    /// <summary>
    /// Создает новый экземпляр RedisDistributedSemaphore
    /// </summary>
    /// <param name="redis">Экземпляр IConnectionMultiplexer для подключения к Redis</param>
    /// <param name="timeoutInSeconds">Время таймаута в секундах</param>
    public RedisDistributedSemaphore(IConnectionMultiplexer redis, int timeoutInSeconds)
    {
        _timeout = TimeSpan.FromSeconds(timeoutInSeconds);
        _redis = redis;
    }

    /// <inheritdoc/>
    public async Task<bool> AcquireAsync(string semaphoreName)
    {
        if (string.IsNullOrEmpty(semaphoreName))
            throw new ArgumentException("Семафор не может быть пустым", nameof(semaphoreName));

        var db = _redis.GetDatabase();
        var acquired = await db.LockTakeAsync(semaphoreName, Environment.MachineName, _timeout);
        return acquired;
    }

    /// <inheritdoc/>
    public void Release(string semaphoreName)
    {
        if (string.IsNullOrEmpty(semaphoreName))
            throw new ArgumentException("Семафор не может быть пустым", nameof(semaphoreName));

        var db = _redis.GetDatabase();
        db.LockRelease(semaphoreName, Environment.MachineName);
    }
}