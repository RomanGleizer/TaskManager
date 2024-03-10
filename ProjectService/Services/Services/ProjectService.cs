using AutoMapper;
using Core.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces;
using Services.ViewModels.ProjectViewModels;

namespace Services.Services;

/// <summary>
/// Сервис для управления проектами
/// </summary>
public class ProjectService(IProjectRepository storeProject, IMapper mapper) : IProjectService
{
    private readonly IProjectRepository _repository = storeProject;
    private readonly IMapper _mapper = mapper;

    private async Task<Project> GetExistingProject(int id)
    {
        var existingProject = await _repository.GetProjectByIdAsync(id);
        return existingProject ?? throw new ValidationException("Project was not found in database", string.Empty);
    }

    /// <inheritdoc/>
    public async Task<ProjectViewModel> GetById(int id)
    {
        var existingProject = await GetExistingProject(id);
        return _mapper.Map<ProjectViewModel>(existingProject);
    }

    /// <inheritdoc/>
    public async Task<ProjectViewModel> Create(CreateProjectViewModel model)
    {
        var mappedProject = _mapper.Map<Project>(model);
        var createdProject = await _repository.AddProjectAsync(mappedProject);
        return _mapper.Map<ProjectViewModel>(createdProject);
    }

    /// <inheritdoc/>
    public async Task<ProjectViewModel> Delete(int id)
    {
        var existingProject = await GetExistingProject(id);
        var removedProject = await _repository.DeleteProjectAsync(existingProject);
        return _mapper.Map<ProjectViewModel>(removedProject);
    }

    /// <inheritdoc/>
    public async Task<ProjectViewModel> Update(int id, UpdateProjectViewModel model)
    {
        var existingProject = await GetExistingProject(id);
        existingProject = existingProject with
        {
            Name = model.Name,
            Description = model.Description,
            LastUpdatedDate = model.LastUpdatedDate,
            MemberIds = model.MemberIds
        };
        var updatedProject = await _repository.UpdateProjectAsync(existingProject);
        return _mapper.Map<ProjectViewModel>(updatedProject);
    }

    /// <inheritdoc/>
    public async Task AddNewTaskInProject(int projectId, int taskId)
    {
        var existingProject = await GetExistingProject(projectId);
        if (!existingProject.TaskIds.Contains(taskId))
        {
            existingProject.TaskIds.Add(taskId);
            await _repository.SaveChangesAsync();
        }
    }
}