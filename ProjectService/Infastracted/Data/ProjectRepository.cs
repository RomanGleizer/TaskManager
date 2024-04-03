using Core.Dal.Base;
using Domain.Entities;
using Infastracted.EF;
using Microsoft.EntityFrameworkCore;

namespace Infastracted.Data;

/// <summary>
/// Реализация интерфейса репозитория для работы с данными проектов
/// </summary>
public class ProjectRepository(ProjectServiceDbContext dbContext) : IProjectRepository<Project, Guid>
{
    private readonly ProjectServiceDbContext _dbContext = dbContext;

    /// <inheritdoc/>
    public async Task<Project?> GetProjectByIdAsync(Guid projectId)
    {
        return await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
    }

    /// <inheritdoc/>
    public async Task<Project> AddProjectAsync(Project project)
    {
        var createdProject = await _dbContext.Projects.AddAsync(project);
        await SaveChangesAsync();

        return createdProject.Entity;
    }

    /// <inheritdoc/>
    public async Task<Project?> UpdateProjectAsync(Project project)
    {
        var entityEntry = _dbContext.Entry(project);
        entityEntry.State = EntityState.Modified;
        await SaveChangesAsync();

        return entityEntry.Entity;
    }

    /// <inheritdoc/>
    public async Task<Project?> DeleteProjectAsync(Project project)
    {
        var removedProject = _dbContext.Projects.Remove(project);
        await SaveChangesAsync();

        return removedProject.Entity;
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
