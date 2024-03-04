using Services.ViewModels;

namespace Services.Interfaces;

public interface IProjectService
{
    Task<ProjectViewModel?> GetById(int id);
    Task<ProjectViewModel?> Create(CreateProjectViewModel model);
    Task<ProjectViewModel?> Delete(int id);
    Task<ProjectViewModel?> Update(int id, UpdateProjectViewModel model);
}
