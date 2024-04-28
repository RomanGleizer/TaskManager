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
    /// Получение доступа к ресурсу, защищаемому семафором, с таймаутом
    /// </summary>
    /// <param name="semaphoreName">Имя семафора</param>
    /// <param name="timeout">Максимальное время ожидания доступа к семафору</param>
    /// <returns>True, если семафор получен успешно, иначе - false</returns>
    Task<bool> AcquireAsync(string semaphoreName, TimeSpan timeout);

    /// <summary>
    /// Освобождение семафора
    /// </summary>
    /// <param name="semaphoreName">Имя семафора</param>
    void Release(string semaphoreName);

    /// <summary>
    /// Освобождение семафора асинхронно
    /// </summary>
    /// <param name="semaphoreName">Имя семафора</param>
    Task ReleaseAsync(string semaphoreName);

    /// <summary>
    /// Получение текущего количества доступных разрешений семафора
    /// </summary>
    /// <param name="semaphoreName">Имя семафора</param>
    /// <returns>Текущее количество доступных разрешений семафора</returns>
    Task<int> GetCountAsync(string semaphoreName);

    /// <summary>
    /// Освобождение всех ресурсов, связанных с семафором
    /// </summary>
    /// <returns>Задача, представляющая асинхронную операцию</returns>
    Task DisposeAsync();
}