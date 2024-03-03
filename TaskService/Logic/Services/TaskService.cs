using Dal.Interfaces;
using Logic.DTO;
using Logic.Interfaces;
using AutoMapper;
using Dal.Entities;
using Core.Exceptions;

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
    public async Task<IList<TaskDTO>> GetAllDtosAsync()
    {
        var allTaskDals = await _unitOfWork.Tasks.GetAllAsync();
        return _mapper.Map<IList<TaskDal>, IList<TaskDTO>>(allTaskDals);
    }

    /// <summary>
    /// Получить объект DTO задачи по идентификатору асинхронно
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    public async Task<TaskDTO> GetDtoByIdAsync(int taskId)
    {
        var taskDal = await GetExistingTaskAsync(taskId);
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
        return _mapper.Map<TaskDTO>(taskDal);
    }

    /// <summary>
    /// Удалить объект DTO задачи асинхронно по идентификатору
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    public async Task<TaskDTO> DeleteDtoAsync(int taskId)
    {
        var existingTaskDal = await GetExistingTaskAsync(taskId);

        await _unitOfWork.Tasks.DeleteAsync(existingTaskDal);
        return _mapper.Map<TaskDTO>(existingTaskDal);
    }

    /// <summary>
    /// Обновить объект DTO задачи асинхронно
    /// </summary>
    /// <param name="taskDTO">DTO задачи для обновления</param>
    /// <param name="taskId">Идентификатор задачи</param>
    public async Task<TaskDTO> UpdateDtoAsync(TaskDTO taskDTO, int taskId)
    {
        var existingTaskDal = await GetExistingTaskAsync(taskId);

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

        await _unitOfWork.Tasks.UpdateAsync(existingTaskDal);
        return _mapper.Map<TaskDTO>(existingTaskDal);
    }

    /// <summary>
    /// Асинхронное получение задачи по Id
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    private async Task<TaskDal> GetExistingTaskAsync(int taskId)
    {
        var existingTaskDal = await _unitOfWork.Tasks.GetByIdAsync(taskId);
        if (existingTaskDal == null)
            throw new ValidationException("Task was not found in database", string.Empty);
        return existingTaskDal;
    }
}
