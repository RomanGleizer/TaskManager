using AutoMapper;
using Core.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces;
using Services.ViewModels.ProjectViewModels;

namespace Services.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository storeProject, IMapper mapper, IMemberRepository memberRepository)
    {
        _repository = storeProject;
        _mapper = mapper;
        _memberRepository = memberRepository;
    }

    public async Task<ProjectViewModel?> GetById(int id)
    {
        var existingProject = await _repository.GetProjectByIdAsync(id);
        if (existingProject == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        return _mapper.Map<ProjectViewModel>(existingProject);
    }

    public async Task<ProjectViewModel?> Create(CreateProjectViewModel model)
    {
        var mappedProject = _mapper.Map<Project>(model);
        var createdProject = await _repository.AddProjectAsync(mappedProject);
        return _mapper.Map<ProjectViewModel>(createdProject);
    }

    public async Task<ProjectViewModel?> Delete(int id)
    {
        var existingProject = await _repository.GetProjectByIdAsync(id);
        if (existingProject == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        var removedProject = await _repository.DeleteProjectAsync(existingProject);
        return _mapper.Map<ProjectViewModel>(removedProject);
    }

    public async Task<ProjectViewModel?> Update(int id, UpdateProjectViewModel model)
    {
        var existingProject = await _repository.GetProjectByIdAsync(id);
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
            var member = await _memberRepository.GetMemberByIdAsync(memberId);
            updatedMembers.Add(member);
        }

        existingProject = existingProject with
        {
            Members = updatedMembers
        };

        var updatedProject = await _repository.UpdateProjectAsync(existingProject);
        return _mapper.Map<ProjectViewModel>(updatedProject);
    }
}
