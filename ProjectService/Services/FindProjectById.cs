using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces.ProjectInterfaces;

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
        // Валидация (если не найден, бросаем исключение);
        return existingProject;
    }
}
