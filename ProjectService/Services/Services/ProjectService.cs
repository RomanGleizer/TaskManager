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

    /// <summary>
    /// Получает информацию о проекте по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    /// <returns>Модель представления проекта или null, если проект не найден</returns>
    public async Task<ProjectViewModel?> GetById(int id)
    {
        var existingProject = await _repository.GetProjectByIdAsync(id);
        if (existingProject == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        return _mapper.Map<ProjectViewModel>(existingProject);
    }

    /// <summary>
    /// Создает новый проект
    /// </summary>
    /// <param name="model">Модель создания проекта</param>
    /// <returns>Модель представления созданного проекта или null, если операция не удалась</returns>
    public async Task<ProjectViewModel?> Create(CreateProjectViewModel model)
    {
        var mappedProject = _mapper.Map<Project>(model);
        var createdProject = await _repository.AddProjectAsync(mappedProject);
        return _mapper.Map<ProjectViewModel>(createdProject);
    }

    /// <summary>
    /// Удаляет проект по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор проекта для удаления</param>
    /// <returns>Модель представления удаленного проекта или null, если операция не удалась</returns>
    public async Task<ProjectViewModel?> Delete(int id)
    {
        var existingProject = await _repository.GetProjectByIdAsync(id);
        if (existingProject == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        var removedProject = await _repository.DeleteProjectAsync(existingProject);
        return _mapper.Map<ProjectViewModel>(removedProject);
    }

    /// <summary>
    /// Обновляет информацию о проекте
    /// </summary>
    /// <param name="id">Идентификатор проекта для обновления</param>
    /// <param name="model">Модель обновления проекта</param>
    /// <returns>Модель представления обновленного проекта или null, если операция не удалась</returns>
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

    /// <summary>
    /// Добавляет новую задачу в проект
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="taskId">Идентификатор добавляемой задачи</param>
    public async Task AddNewTaskInProject(int projectId, int taskId)
    {
        var existingProject = await _repository.GetProjectByIdAsync(projectId);
        if (existingProject == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        if (!existingProject.TaskIds.Contains(taskId))
            existingProject.TaskIds.Add(taskId);

        await _repository.SaveChangesAsync();
    }
}