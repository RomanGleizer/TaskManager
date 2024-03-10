using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Интерфейс для работы с данными проектов
/// </summary>
public interface IProjectRepository
{
    /// <summary>
    /// Получает проект по его идентификатору
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <returns>Объект проекта или null, если проект не найден</returns>
    Task<Project?> GetProjectByIdAsync(int projectId);

    /// <summary>
    /// Добавляет новый проект
    /// </summary>
    /// <param name="project">Проект для добавления</param>
    /// <returns>Добавленный проект или null, если операция не удалась</returns>
    Task<Project> AddProjectAsync(Project project);

    /// <summary>
    /// Удаляет проект
    /// </summary>
    /// <param name="project">Проект для удаления</param>
    /// <returns>Удаленный проект или null, если операция не удалась</returns>
    Task<Project?> DeleteProjectAsync(Project project);

    /// <summary>
    /// Обновляет данные проекта
    /// </summary>
    /// <param name="project">Проект для обновления</param>
    /// <returns>Обновленный проект или null, если операция не удалась</returns>
    Task<Project?> UpdateProjectAsync(Project project);

    /// <summary>
    /// Асинхронно сохраняет примененные изменения
    /// </summary>
    Task SaveChangesAsync();
}
