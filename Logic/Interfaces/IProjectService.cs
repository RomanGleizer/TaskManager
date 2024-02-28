using Dal.Entities;
using Logic.DTO;

namespace Logic.Interfaces;

/// <summary>
/// Сервис задач, предназначенный для операций над проектами
/// </summary>
public interface IProjectService
{
    /// <summary>
    /// Получает задачу по идентификатору асинхронно.
    /// </summary>
    Task<ProjectDTO> GetProjectByIdAsync(int id);

    /// <summary>
    /// Получает все задачи.
    /// </summary>
    IEnumerable<ProjectDTO> GetAllProjects();

    /// <summary>
    /// Создает новую задачу асинхронно.
    /// </summary>
    Task<ProjectDTO> CreateProjectAsync(ProjectDTO project);

    /// <summary>
    /// Удаляет задачу по идентификатору асинхронно.
    /// </summary>
    Task<ProjectDTO> DeleteProjectAsync(int id);

    /// <summary>
    /// Обновляет задачу по идентификатору асинхронно.
    /// </summary>
    Task<ProjectDTO> UpdateProjectAsync(ProjectDTO project, int id);
}
