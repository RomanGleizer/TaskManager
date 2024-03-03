using Domain.Entities;
using Domain.Interfaces;

namespace Infastracted.Data;

public class ProjectRepository : IStoreProject
{
    // Логика для работы с EF

    public Task<Project?> GetProjectByIdAsync(int project)
    {
        throw new NotImplementedException();
    }

    public Task<int> AddProjectAsync(Project project)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateProjectAsync(int projectId, Project project)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteProjectAsync(Project project)
    {
        throw new NotImplementedException();
    }
}
