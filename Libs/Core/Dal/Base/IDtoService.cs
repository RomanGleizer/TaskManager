namespace Core.Dal.Base;

public interface IDtoService<TEntity, TId>
    where TEntity : IBaseDTO<TId>
{
    /// <summary>
    /// Получает объект DTO по идентификатору асинхронно
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    Task<TEntity> GetDtoByIdAsync(TId id);

    /// <summary>
    /// Получает все объекты DTO
    /// </summary>
    Task<IList<TEntity>> GetAllDtosAsync();

    /// <summary>
    /// Создает объект DTO асинхронно
    /// </summary>
    /// <param name="taskDto">DTO объект для создания</param>
    Task<TEntity> CreateDtoAsync(TEntity taskDto);

    /// <summary>
    /// Удаляет объект DTO асинхронно по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта для удаления</param>
    Task<TEntity> DeleteDtoAsync(TId id);

    /// <summary>
    /// Обновляет объект DTO асинхронно
    /// </summary>
    /// <param name="taskDto">DTO объект для обновления</param>
    /// <param name="id">Идентификатор объекта для обновления</param>
    Task<TEntity> UpdateDtoAsync(TEntity taskDto, TId id);
}