using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreProject
{
    Task<Project?> GetProjectByIdAsync(int projectId);
    Task<int> AddProjectAsync(Project project);
    Task<int> DeleteProjectAsync(Project project);
    Task<int> UpdateProjectAsync(int projectId, Project project);
}
