using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces.ProjectInterfaces;

namespace Services;

public class CreateProject : ICreateProject
{
    private readonly IStoreProject _storeProject;

    public CreateProject(IStoreProject storeProject)
    {
        _storeProject = storeProject;
    }

    public async Task<int> CreateProjectAsync(Project project)
    {
        return await _storeProject.AddProjectAsync(project);
    }
}
