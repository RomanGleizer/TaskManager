namespace Core.Dal.Base;

/// <summary>
///     Интерфейс для работы с данными проектов
/// </summary>
public interface IProjectRepository<TEntity, TId>
    where TEntity : IBaseEntity<Guid>
{
    /// <summary>
    ///     Получает проект по его идентификатору
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <returns>Объект проекта или null, если проект не найден</returns>
    Task<TEntity?> GetProjectByIdAsync(TId projectId);

    /// <summary>
    ///     Добавляет новый проект
    /// </summary>
    /// <param name="project">Проект для добавления</param>
    /// <returns>Добавленный проект или null, если операция не удалась</returns>
    Task<TEntity> AddProjectAsync(TEntity project);

    /// <summary>
    ///     Удаляет проект
    /// </summary>
    /// <param name="project">Проект для удаления</param>
    /// <returns>Удаленный проект или null, если операция не удалась</returns>
    Task<TEntity?> DeleteProjectAsync(TEntity project);

    /// <summary>
    ///     Обновляет данные проекта
    /// </summary>
    /// <param name="project">Проект для обновления</param>
    /// <returns>Обновленный проект или null, если операция не удалась</returns>
    Task<TEntity?> UpdateProjectAsync(TEntity project);

    /// <summary>
    ///     Асинхронно сохраняет примененные изменения
    /// </summary>
    Task SaveChangesAsync();
}