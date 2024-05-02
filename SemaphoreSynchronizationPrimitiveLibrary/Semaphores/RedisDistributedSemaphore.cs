using SemaphoreSynchronizationPrimitiveLibrary.Interfaces;
using StackExchange.Redis;

namespace SemaphoreSynchronizationPrimitiveLibrary.Semaphores;

/// <summary>
/// Реализация распределенного семафора с использованием Redis
/// </summary>
public class RedisDistributedSemaphore(IConnectionMultiplexer redis, int timeoutInSeconds) 
    : IDistributedSemaphore
{
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(timeoutInSeconds);
    private readonly IConnectionMultiplexer _redis = redis;
    
    /// <inheritdoc />
    public async Task<bool> AcquireAsync(string semaphoreName)
    {
        if (string.IsNullOrEmpty(semaphoreName))
            throw new ArgumentException("Семафор не может быть пустым", nameof(semaphoreName));

        var db = _redis.GetDatabase();
        return await db.LockTakeAsync(semaphoreName, Environment.MachineName, _timeout);
    }

    /// <inheritdoc />
    public async Task<bool> AcquireAsync(string semaphoreName, TimeSpan timeout)
    {
        if (string.IsNullOrEmpty(semaphoreName))
            throw new ArgumentException("Семафор не может быть пустым", nameof(semaphoreName));

        var db = _redis.GetDatabase();
        return await db.LockTakeAsync(semaphoreName, Environment.MachineName, timeout);
    }

    /// <inheritdoc />
    public void Release(string semaphoreName)
    {
        if (string.IsNullOrEmpty(semaphoreName))
            throw new ArgumentException("Семафор не может быть пустым", nameof(semaphoreName));

        var db = _redis.GetDatabase();
        db.LockRelease(semaphoreName, RedisValue.Null);
    }

    /// <inheritdoc />
    public async Task ReleaseAsync(string semaphoreName)
    {
        if (string.IsNullOrEmpty(semaphoreName))
            throw new ArgumentException("Семафор не может быть пустым", nameof(semaphoreName));

        var db = _redis.GetDatabase();
        await db.LockReleaseAsync(semaphoreName, Environment.MachineName);
    }

    /// <inheritdoc />
    public async Task<int> GetCountAsync(string semaphoreName)
    {
        if (string.IsNullOrEmpty(semaphoreName))
            throw new ArgumentException("Семафор не может быть пустым", nameof(semaphoreName));

        var db = _redis.GetDatabase();
        var lockValue = await db.LockQueryAsync(semaphoreName);
        return lockValue.IsNull ? 0 : 1;
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        await Task.CompletedTask;
    }
}