namespace Core.Dal.Base;

/// <summary>
///     Представляет общий интерфейс репозитория для работы с сущностями
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TId">Тип идентификатора сущности</typeparam>
/// <remarks>
///     Для использования этого интерфейса сущность должна реализовывать интерфейс IBaseEntity с указанным типом
///     идентификатора
/// </remarks>
public interface IRepository<TEntity, in TId>
    where TEntity : IBaseEntity<TId>
{
    /// <summary>
    ///     Асинхронно получает все сущности
    /// </summary>
    /// <returns>Список всех сущностей</returns>
    Task<IList<TEntity>> GetAllAsync();

    /// <summary>
    ///     Асинхронно получает сущность по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Сущность с указанным идентификатором или null, если не найдена</returns>
    Task<TEntity?> GetByIdAsync(TId id);

    /// <summary>
    ///     Асинхронно создает новую сущность
    /// </summary>
    /// <param name="entity">Создаваемая сущность</param>
    Task<TEntity?> CreateAsync(TEntity entity);

    /// <summary>
    ///     Асинхронно обновляет существующую сущность
    /// </summary>
    /// <param name="entity">Обновляемая сущность</param>
    Task<TEntity?> UpdateAsync(TEntity entity);

    /// <summary>
    ///     Асинхронно удаляет сущность
    /// </summary>
    /// <param name="entity">Удаляемая сущность</param>
    Task<TEntity?> DeleteAsync(TEntity entity);

    /// <summary>
    ///     Асинхронно сохраняет изменения
    /// </summary>
    Task SaveChangesAsync();
}