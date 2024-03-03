using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces.Project_Interfaces;

namespace Services;

public class DeleteProject : IDeleteProject
{
    private readonly IStoreProject _storeProject;

    public DeleteProject(IStoreProject storeProject)
    {
        _storeProject = storeProject;
    }

    public async Task<int> DeleteProjectAsync(int id)
    {
        var existingProject = await _storeProject.GetProjectByIdAsync(id);
        // Валидация (если не найден, бросаем исключение);

        return await _storeProject.DeleteProjectAsync(existingProject);
    }
}
