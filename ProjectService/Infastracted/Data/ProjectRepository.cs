using Domain.Entities;
using Domain.Interfaces;
using Infastracted.EF;
using Microsoft.EntityFrameworkCore;

namespace Infastracted.Data;

/// <summary>
/// Реализация интерфейса репозитория для работы с данными проектов
/// </summary>
public class ProjectRepository : IProjectRepository
{
    private readonly ProjectServiceDbContext _dbContext;

    public ProjectRepository(ProjectServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получает проект по его идентификатору
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <returns>Объект проекта или null, если проект не найден</returns>
    public async Task<Project?> GetProjectByIdAsync(int projectId)
    {
        return await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
    }

    /// <summary>
    /// Добавляет новый проект
    /// </summary>
    /// <param name="project">Проект для добавления</param>
    /// <returns>Добавленный проект или null, если операция не удалась</returns>
    public async Task<Project?> AddProjectAsync(Project project)
    {
        var createdProject = await _dbContext.Projects.AddAsync(project);
        await _dbContext.SaveChangesAsync();

        return createdProject.Entity;
    }

    /// <summary>
    /// Обновляет данные проекта
    /// </summary>
    /// <param name="project">Проект для обновления</param>
    /// <returns>Обновленный проект или null, если операция не удалась</returns>
    public async Task<Project?> UpdateProjectAsync(Project project)
    {
        var entityEntry = _dbContext.Entry(project);
        entityEntry.State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return entityEntry.Entity;
    }

    /// <summary>
    /// Удаляет проект
    /// </summary>
    /// <param name="project">Проект для удаления</param>
    /// <returns>Удаленный проект или null, если операция не удалась</returns>
    public async Task<Project?> DeleteProjectAsync(Project project)
    {
        var removedProject = _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();

        return removedProject.Entity;
    }
}
