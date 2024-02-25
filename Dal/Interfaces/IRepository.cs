namespace Dal.Interfaces;

/// <summary>
/// Интерфейс репозитория для операций с сущностями
/// </summary>
/// <typeparam name="T">Сущность, реализующая интерфейс IDbEntity</typeparam>
public interface IRepository<T, I> where T : IDbEntity<I>
{
    /// <summary>
    /// Возвращает все сущности данного типа.
    /// </summary>
    IEnumerable<T> GetAllTasks();

    /// <summary>
    /// Возвращает сущность по заданному идентификатору.
    /// </summary>
    Task<T?> GetTaskById(int id);

    /// <summary>
    /// Создает новую сущность в хранилище данных.
    /// </summary>
    Task CreateTaskAsync(T item);

    /// <summary>
    /// Обновляет существующую сущность в хранилище данных.
    /// </summary>
    void UpdateTaskAsync(T item);

    /// <summary>
    /// Удаляет сущность с заданным идентификатором из хранилища данных.
    /// </summary>
    Task DeleteTaskAsync(int id);
}
