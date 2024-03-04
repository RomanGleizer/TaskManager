using AutoMapper;
using Core.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces;
using Services.ViewModels;

namespace Services.Services;

public class ProjectService : IProjectService
{
    private readonly IStoreProject _storeProject;
    private readonly IMapper _mapper;

    public ProjectService(IStoreProject storeProject, IMapper mapper)
    {
        _storeProject = storeProject;
        _mapper = mapper;
    }

    public async Task<ProjectViewModel?> GetById(int id)
    {
        var existingProject = await _storeProject.GetProjectByIdAsync(id);
        if (existingProject == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        return _mapper.Map<ProjectViewModel>(existingProject);
    }

    public async Task<ProjectViewModel?> Create(CreateProjectViewModel model)
    {
        var mappedProject = _mapper.Map<Project>(model);
        var createdProject = await _storeProject.AddProjectAsync(mappedProject);
        return _mapper.Map<ProjectViewModel>(createdProject);
    }

    public async Task<ProjectViewModel?> Delete(int id)
    {
        var existingProject = await _storeProject.GetProjectByIdAsync(id);
        if (existingProject == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        var removedProject = await _storeProject.DeleteProjectAsync(existingProject);
        return _mapper.Map<ProjectViewModel>(removedProject);
    }

    public async Task<ProjectViewModel?> Update(int id, UpdateProjectViewModel model)
    {
        var existingProject = await _storeProject.GetProjectByIdAsync(id);
        if (existingProject == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        existingProject = existingProject with
        {
            Name = model.Name,
            Description = model.Description,
            LastUpdatedDate = model.LastUpdatedDate,
            MemberIds = model.MemberIds
        };

        var updatedMembers = new List<Member>();
        foreach (var memberId in model.MemberIds)
        {
            /*
             * var member = await _storeMember.GetMemberByIdAsync(memberId);
             * updatedMembers.Add(member);
            */
        }

        existingProject = existingProject with
        {
            Members = updatedMembers
        };

        var updatedProject = await _storeProject.UpdateProjectAsync(existingProject);

        return _mapper.Map<ProjectViewModel>(updatedProject);
    }
}
