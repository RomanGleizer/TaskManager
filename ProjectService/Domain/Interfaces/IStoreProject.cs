using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreProject
{
    Task<Project?> GetProjectByIdAsync(int projectId);
    Task AddProjectAsync(Project project);
    Task DeleteProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
}
