using Dal.Interfaces;
using Logic.DTO;
using Logic.Interfaces;
using Logic.Infrastructure;
using AutoMapper;
using Dal.Entities;

namespace Logic.Services;

/// <summary>
/// Сервис для работы с задачами
/// </summary>
public class TaskService : IDtoService<TaskDTO, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор сервиса задач
    /// </summary>
    /// <param name="unitOfWork">Единица работы с данными</param>
    /// <param name="mapper">Маппер для преобразования между объектами</param>
    public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить все объекты DTO задач
    /// </summary>
    public IEnumerable<TaskDTO> GetAllDtos()
    {
        var allTaskDals = _unitOfWork.Tasks.GetAll();
        return _mapper.Map<IEnumerable<TaskDal>, IEnumerable<TaskDTO>>(allTaskDals);
    }

    /// <summary>
    /// Получить объект DTO задачи по идентификатору асинхронно
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    public async Task<TaskDTO> GetDtoByIdAsync(int id)
    {
        var taskDal = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (taskDal == null)
            throw new ValidationException("Task was not found in database", string.Empty);
        return _mapper.Map<TaskDTO>(taskDal);
    }

    /// <summary>
    /// Создать объект DTO задачи асинхронно
    /// </summary>
    /// <param name="taskDTO">DTO задачи для создания</param>
    public async Task<TaskDTO> CreateDtoAsync(TaskDTO taskDTO)
    {
        var taskDal = _mapper.Map<TaskDal>(taskDTO);

        await _unitOfWork.Tasks.CreateAsync(taskDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TaskDTO>(taskDal);
    }

    /// <summary>
    /// Удалить объект DTO задачи асинхронно по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    public async Task<TaskDTO> DeleteDtoAsync(int id)
    {
        var existingTaskDal = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (existingTaskDal == null)
            throw new ValidationException("Task was not found in database", string.Empty);

        _unitOfWork.Tasks.Delete(existingTaskDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TaskDTO>(existingTaskDal);
    }

    /// <summary>
    /// Обновить объект DTO задачи асинхронно
    /// </summary>
    /// <param name="taskDTO">DTO задачи для обновления</param>
    /// <param name="taskId">Идентификатор задачи</param>
    public async Task<TaskDTO> UpdateDtoAsync(TaskDTO taskDTO, int taskId)
    {
        var existingTaskDal = await _unitOfWork.Tasks.GetByIdAsync(taskId);
        if (existingTaskDal == null)
            throw new ValidationException("Task was not found in database", string.Empty);

        existingTaskDal.Name = taskDTO.Name;
        existingTaskDal.Description = taskDTO.Description;
        existingTaskDal.ExecutionStatus = taskDTO.ExecutionStatus;
        existingTaskDal.CreatedDate = taskDTO.CreatedDate;
        existingTaskDal.LastUpdateDate = taskDTO.LastUpdateDate;
        existingTaskDal.PerformerIds = taskDTO.PerformerIds;
        existingTaskDal.CommentIds = taskDTO.CommentIds;
        existingTaskDal.ProjectId = taskDTO.ProjectId;

        foreach (var commentId in taskDTO.CommentIds)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(commentId);
            if (comment != null)
                existingTaskDal.Comments.Add(comment);
        }

        var project = await _unitOfWork.Projects.GetByIdAsync(taskDTO.ProjectId);
        if (project != null)
            existingTaskDal.Project = project;

        _unitOfWork.Tasks.Update(existingTaskDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TaskDTO>(existingTaskDal);
    }
}
