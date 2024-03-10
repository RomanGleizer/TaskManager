using Core.Dal.Base;

namespace Dal.Interfaces;

/// <summary>
/// Интерфейс репозитория для операций с сущностями
/// </summary>
/// <typeparam name="TEntity">Сущность, реализующая интерфейс IDbEntity</typeparam>
/// <typeparam name="TIndex">Тип уникального идентификатора сущности</typeparam>
public interface IRepository<TEntity, TIndex> 
    where TEntity : IBaseEntity<TIndex>
{
    /// <summary>
    /// Возвращает все сущности данного типа
    /// </summary>
    Task<IList<TEntity>> GetAllAsync();

    /// <summary>
    /// Возвращает сущность по заданному идентификатору
    /// </summary>
    Task<TEntity?> GetByIdAsync(TIndex id);

    /// <summary>
    /// Создает новую сущность в хранилище данных
    /// </summary>
    Task CreateAsync(TEntity item);

    /// <summary>
    /// Обновляет существующую сущность в хранилище данных
    /// </summary>
    Task UpdateAsync(TEntity item);

    /// <summary>
    /// Удаляет сущность с заданным идентификатором из хранилища данных
    /// </summary>
    Task DeleteAsync(TEntity item);
}
