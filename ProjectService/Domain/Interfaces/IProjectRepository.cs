using Domain.Entities;

namespace Domain.Interfaces;

public interface IProjectRepository
{
    Task<Project?> GetProjectByIdAsync(int projectId);
    Task<Project?> AddProjectAsync(Project project);
    Task<Project?> DeleteProjectAsync(Project project);
    Task<Project?> UpdateProjectAsync(Project project);
}
