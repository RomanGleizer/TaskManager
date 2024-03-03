using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces.ProjectInterfaces;
using Core.Exceptions;

namespace Services;

public class FindProjectById : IFindProject
{
    private readonly IStoreProject _storeProject;

    public FindProjectById(IStoreProject storeProject)
    {
        _storeProject = storeProject;
    }

    public async Task<Project?> GetProjectByIdAsync(int projectId)
    {
        var existingProject = await _storeProject.GetProjectByIdAsync(projectId);
        if (existingProject == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        return existingProject;
    }
}
