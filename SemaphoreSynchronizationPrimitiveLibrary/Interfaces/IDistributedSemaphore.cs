namespace SemaphoreSynchronizationPrimitiveLibrary.Interfaces;

/// <summary>
/// Интерфейс для реализации распределенного семафора
/// </summary>
public interface IDistributedSemaphore
{
    /// <summary>
    /// Получение доступа к ресурсу, защищаемому семафором
    /// </summary>
    /// <param name="semaphoreName">Имя семафора</param>
    /// <returns>True, если семафор получен успешно, иначе - false</returns>
    Task<bool> AcquireAsync(string semaphoreName);

    /// <summary>
    /// Освобождает семафор
    /// </summary>
    /// <param name="semaphoreName">Имя семафора</param>
    void Release(string semaphoreName);
}