using AutoMapper;
using ConnectionLibrary.ConnectionServices.DtoModels.AddProjectToListOfUserProjects;
using ConnectionLibrary.ConnectionServices.Interfaces;
using Core.Dal.Base;
using ProjectsMicroservice.ProjectsMicroserviceApplication.Interfaces;
using ProjectsMicroservice.ProjectsMicroserviceApplication.ViewModels.ProjectViewModels;
using ProjectsMicroservice.ProjectsMicroserviceDomain.Entities;

namespace ProjectsMicroservice.ProjectsMicroserviceApplication.Services;

/// <summary>
///     Сервис для управления проектами
/// </summary>
public class ProjectService(
    IProjectRepository<Project, Guid> repository,
    IMapper mapper,
    IUserConnectionService userConnectionService) : IProjectService
{
    /// <inheritdoc />
    public async Task<ProjectViewModel> GetById(Guid id)
    {
        var existingProject = await GetExistingProject(id);
        return mapper.Map<ProjectViewModel>(existingProject);
    }

    /// <inheritdoc />
    public async Task<ProjectViewModel> Create(CreateProjectViewModel model)
    {
        var mappedProject = mapper.Map<Project>(model);
        var createdProject = await repository.AddProjectAsync(mappedProject);

        // Самый первый участник - создатель проекта (в дальнейшем здесь можно будет изменить логику на проверку роли)
        var projectCreatorId = createdProject.MemberIds.FirstOrDefault();

        // При создании проекта он появляется в списке проектов создателя
        await userConnectionService.AddProjectIdToListOfUserProjectIds(new AddProjectToListOfUserProjectsRequest
        {
            ProjectId = createdProject.Id,
            MemberId = projectCreatorId
        });

        return mapper.Map<ProjectViewModel>(createdProject);
    }

    /// <inheritdoc />
    public async Task<ProjectViewModel> Delete(Guid id)
    {
        var existingProject = await GetExistingProject(id);
        var removedProject = await repository.DeleteProjectAsync(existingProject);
        return mapper.Map<ProjectViewModel>(removedProject);
    }

    /// <inheritdoc />
    public async Task<ProjectViewModel> Update(Guid id, UpdateProjectViewModel model)
    {
        var existingProject = await GetExistingProject(id);
        existingProject = existingProject with
        {
            Name = model.Name,
            Description = model.Description,
            LastUpdatedDate = model.LastUpdatedDate,
            MemberIds = model.MemberIds
        };
        var updatedProject = await repository.UpdateProjectAsync(existingProject);
        return mapper.Map<ProjectViewModel>(updatedProject);
    }

    private async Task<Project> GetExistingProject(Guid id)
    {
        var existingProject = await repository.GetProjectByIdAsync(id);
        return existingProject ?? throw new Exception("Не удалось найти существующий проект БД");
    }
}