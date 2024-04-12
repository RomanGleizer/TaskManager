using Core.Dal.Base;
using Microsoft.EntityFrameworkCore;
using ProjectsMicroservice.ProjectsMicroserviceDomain.Entities;
using ProjectsMicroservice.ProjectsMicroserviceInfrastructure.EntityFramework;

namespace ProjectsMicroservice.ProjectsMicroserviceInfrastructure.Data;

/// <summary>
///     Реализация интерфейса репозитория для работы с данными проектов
/// </summary>
public class ProjectRepository(ProjecstMicroserviceDbContext dbContext) : IProjectRepository<Project, Guid>
{
    /// <inheritdoc />
    public async Task<Project?> GetProjectByIdAsync(Guid projectId)
    {
        return await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
    }

    /// <inheritdoc />
    public async Task<Project> AddProjectAsync(Project project)
    {
        var createdProject = await dbContext.Projects.AddAsync(project);
        await SaveChangesAsync();

        return createdProject.Entity;
    }

    /// <inheritdoc />
    public async Task<Project?> UpdateProjectAsync(Project project)
    {
        var entityEntry = dbContext.Entry(project);
        entityEntry.State = EntityState.Modified;
        await SaveChangesAsync();

        return entityEntry.Entity;
    }

    /// <inheritdoc />
    public async Task<Project?> DeleteProjectAsync(Project project)
    {
        var removedProject = dbContext.Projects.Remove(project);
        await SaveChangesAsync();

        return removedProject.Entity;
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}