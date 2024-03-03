using Domain.Entities;
using Domain.Interfaces;
using Infastracted.EF;
using Microsoft.EntityFrameworkCore;

namespace Infastracted.Data;

public class ProjectRepository : IStoreProject
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

    public async Task AddProjectAsync(Project project)
    {
        await _dbContext.Projects.AddAsync(project);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateProjectAsync(Project project)
    {
        _dbContext.Entry(project).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(Project project)
    {
        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
    }
}
