using AutoMapper;
using Dal.Entities;
using Dal.Interfaces;
using Logic.DTO;
using Logic.Infrastructure;
using Logic.Interfaces;

namespace Logic.Services;

/// <summary>
/// Сервис для работы с проектами
/// </summary>
public class ProjectService : IDtoService<ProjectDTO, int>, IProjectDtoManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор сервиса проектов
    /// </summary>
    /// <param name="unitOfWork">Единица работы с данными</param>
    /// <param name="mapper">Маппер для преобразования между объектами</param>
    public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить все объекты DTO проектов
    /// </summary>
    public async Task<IList<ProjectDTO>> GetAllDtosAsync()
    {
        var allProjectDals = await _unitOfWork.Projects.GetAllAsync();
        return _mapper.Map<IList<ProjectDal>, IList<ProjectDTO>>(allProjectDals);
    }

    /// <summary>
    /// Получить объект DTO проекта по идентификатору асинхронно
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    public async Task<ProjectDTO> GetDtoByIdAsync(int id)
    {
        var projectDal = await GetExistingProjectAsync(id);
        return _mapper.Map<ProjectDTO>(projectDal);
    }

    /// <summary>
    /// Создать объект DTO проекта асинхронно
    /// </summary>
    /// <param name="projectDTO">DTO проекта для создания</param>
    public async Task<ProjectDTO> CreateDtoAsync(ProjectDTO projectDTO)
    {
        var projectDal = _mapper.Map<ProjectDal>(projectDTO);

        await _unitOfWork.Projects.CreateAsync(projectDal);
        return _mapper.Map<ProjectDTO>(projectDal);
    }

    /// <summary>
    /// Удалить объект DTO проекта асинхронно по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    public async Task<ProjectDTO> DeleteDtoAsync(int id)
    {
        var existingProjectDal = await GetExistingProjectAsync(id);

        await _unitOfWork.Projects.DeleteAsync(existingProjectDal);
        return _mapper.Map<ProjectDTO>(existingProjectDal);
    }

    /// <summary>
    /// Обновить объект DTO проекта асинхронно
    /// </summary>
    /// <param name="projectDTO">DTO проекта для обновления</param>
    /// <param name="projectId">Идентификатор проекта</param>
    public async Task<ProjectDTO> UpdateDtoAsync(ProjectDTO projectDTO, int projectId)
    {
        var existingProjectDal = await GetExistingProjectAsync(projectId);

        existingProjectDal.Name = projectDTO.Name;
        existingProjectDal.Description = projectDTO.Description;
        existingProjectDal.CreationDate = projectDTO.CreationDate;
        existingProjectDal.LastModifidedDate = projectDTO.LastModifidedDate;
        existingProjectDal.ParticipantIds = projectDTO.ParticipantIds;
        existingProjectDal.TaskIds = projectDTO.TaskIds;

        foreach (var participantId in projectDTO.ParticipantIds)
        {
            var participant = await _unitOfWork.Users.GetByIdAsync(participantId);
            if (participant != null)
                existingProjectDal.Participants.Add(participant);
        }

        foreach (var taskId in projectDTO.TaskIds)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(taskId);
            if (task != null)
                existingProjectDal.Tasks.Add(task);
        }

        await _unitOfWork.Projects.UpdateAsync(existingProjectDal);
        return _mapper.Map<ProjectDTO>(existingProjectDal);
    }

    /// <summary>
    /// Добавить участника в проект асинхронно
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="participantId">Идентификатор участника</param>
    public async Task<ProjectDTO> AddParticipantAsync(int projectId, string participantId)
    {
        var existingUserDal = await _unitOfWork.Users.GetByIdAsync(participantId);
        if (existingUserDal == null)
            throw new ValidationException("User was not found in database", string.Empty);

        var existingProjectDal = await GetExistingProjectAsync(projectId);

        existingProjectDal.Participants.Add(existingUserDal);
        existingProjectDal.ParticipantIds.Add(participantId);

        await _unitOfWork.Projects.UpdateAsync(existingProjectDal);
        return _mapper.Map<ProjectDTO>(existingProjectDal);
    }

    /// <summary>
    /// Получить всех участников проекта асинхронно
    /// </summary>
    /// <param name="projectId">>Идентификатор проекта</param>
    public async Task<IList<UserDTO>> GetAllParticipantsAsync(int projectId)
    {
        var existingProjectDal = await GetExistingProjectAsync(projectId);
        return _mapper.Map<IList<UserDTO>>(existingProjectDal.Participants);
    }

    /// <summary>
    /// Асинхронное получение проекта по Id
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    private async Task<ProjectDal> GetExistingProjectAsync(int projectId)
    {
        var existingProjectDal = await _unitOfWork.Projects.GetByIdAsync(projectId);
        if (existingProjectDal == null)
            throw new ValidationException("Project was not found in database", string.Empty);
        return existingProjectDal;
    }
}
