using Core.Dal.Base;

namespace Dal.Interfaces;

/// <summary>
/// Интерфейс репозитория для операций с сущностями
/// </summary>
/// <typeparam name="T">Сущность, реализующая интерфейс IDbEntity</typeparam>
/// <typeparam name="I">Тип уникального идентификатора сущности</typeparam>
public interface IRepository<T, I> 
    where T : IBaseEntityDal<I>
{
    /// <summary>
    /// Возвращает все сущности данного типа.
    /// </summary>
    IEnumerable<T> GetAll();

    /// <summary>
    /// Возвращает сущность по заданному идентификатору.
    /// </summary>
    Task<T?> GetByIdAsync(I id);

    /// <summary>
    /// Создает новую сущность в хранилище данных.
    /// </summary>
    Task CreateAsync(T item);

    /// <summary>
    /// Обновляет существующую сущность в хранилище данных.
    /// </summary>
    void Update(T item);

    /// <summary>
    /// Удаляет сущность с заданным идентификатором из хранилища данных.
    /// </summary>
    void Delete(T item);
}
