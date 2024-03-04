using Domain.Entities;
using Domain.Interfaces;
using Infastracted.EF;
using Microsoft.EntityFrameworkCore;

namespace Infastracted.Data;

public class ProjectRepository : IProjectRepository
{
    private readonly ProjectServiceDbContext _dbContext;

    public ProjectRepository(ProjectServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Project?> GetProjectByIdAsync(int projectId)
    {
        return await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task<Project?> AddProjectAsync(Project project)
    {
        var createdProject = await _dbContext.Projects.AddAsync(project);
        await _dbContext.SaveChangesAsync();

        return createdProject.Entity;
    }

    public async Task<Project?> UpdateProjectAsync(Project project)
    {
        var entityEntry = _dbContext.Entry(project);
        entityEntry.State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<Project?> DeleteProjectAsync(Project project)
    {
        var removedProject = _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();

        return removedProject.Entity;
    }
}
