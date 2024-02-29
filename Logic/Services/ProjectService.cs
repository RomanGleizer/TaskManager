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
    public IEnumerable<ProjectDTO> GetAllDtos()
    {
        var allProjectDals = _unitOfWork.Projects.GetAll();
        return _mapper.Map<IEnumerable<ProjectDal>, IEnumerable<ProjectDTO>>(allProjectDals);
    }

    /// <summary>
    /// Получить объект DTO проекта по идентификатору асинхронно
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    public async Task<ProjectDTO> GetDtoByIdAsync(int id)
    {
        var projectDal = await _unitOfWork.Projects.GetByIdAsync(id);
        if (projectDal == null)
            throw new ValidationException("Project was not found in database", string.Empty);
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
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProjectDTO>(projectDal);
    }

    /// <summary>
    /// Удалить объект DTO проекта асинхронно по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    public async Task<ProjectDTO> DeleteDtoAsync(int id)
    {
        var existingProjectDal = await _unitOfWork.Projects.GetByIdAsync(id);
        if (existingProjectDal == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        _unitOfWork.Projects.Delete(existingProjectDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProjectDTO>(existingProjectDal);
    }

    /// <summary>
    /// Обновить объект DTO проекта асинхронно
    /// </summary>
    /// <param name="projectDTO">DTO проекта для обновления</param>
    /// <param name="projectId">Идентификатор проекта</param>
    public async Task<ProjectDTO> UpdateDtoAsync(ProjectDTO projectDTO, int projectId)
    {
        var existingProjectDal = await _unitOfWork.Projects.GetByIdAsync(projectId);
        if (existingProjectDal == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        existingProjectDal.Name = projectDTO.Name;
        existingProjectDal.Description = projectDTO.Description;
        existingProjectDal.CreationDate = projectDTO.CreationDate;
        existingProjectDal.LastModifidedDate = projectDTO.LastModifidedDate;
        existingProjectDal.ParticipantIds = projectDTO.ParticipantIds;
        existingProjectDal.TaskIds = projectDTO.TaskIds;

        // Id обновляем, а сами задачи и участников - нет.

        _unitOfWork.Projects.Update(existingProjectDal);
        await _unitOfWork.SaveChangesAsync();
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

        var existingProjectDal = await _unitOfWork.Projects.GetByIdAsync(projectId);
        if (existingProjectDal == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        existingProjectDal.Participants.Add(existingUserDal);
        existingProjectDal.ParticipantIds.Add(participantId);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProjectDTO>(existingProjectDal);
    }
}
