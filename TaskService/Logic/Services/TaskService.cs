using Dal.Interfaces;
using Logic.DTO;
using Core.Dal.Base;
using AutoMapper;
using Dal.Entities;
using Core.Exceptions;
using ConnectionLib.ConnectionServices.Interfaces;
using ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;
using ConnectionLib.ConnectionServices.DtoModels.ProjectById;

namespace Logic.Services;

/// <summary>
/// Сервис для работы с задачами
/// </summary>
/// <remarks>
/// Конструктор сервиса задач
/// </remarks>
/// <param name="unitOfWork">Единица работы с данными</param>
/// <param name="mapper">Маппер для преобразования между объектами</param>
public class TaskService(IUnitOfWork unitOfWork, IMapper mapper, IProjectConnectionService projectConnectionService) : IDtoService<TaskDTO, int>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IProjectConnectionService _projectConnectionService = projectConnectionService;

    /// <inheritdoc/>
    public async Task<IList<TaskDTO>> GetAllDtosAsync()
    {
        var allTaskDals = await _unitOfWork.Tasks.GetAllAsync();
        return _mapper.Map<IList<TaskDal>, IList<TaskDTO>>(allTaskDals);
    }

    /// <inheritdoc/>
    public async Task<TaskDTO> GetDtoByIdAsync(int taskId)
    {
        var taskDal = await GetExistingTaskAsync(taskId);
        return _mapper.Map<TaskDTO>(taskDal);
    }

    /// <inheritdoc/>
    public async Task<TaskDTO> CreateDtoAsync(TaskDTO taskDTO)
    {
        var taskDal = _mapper.Map<TaskDal>(taskDTO);
        await _unitOfWork.Tasks.CreateAsync(taskDal);

        /// Получаем проект по его Id из микросервиса ProjectService
        var project = await _projectConnectionService.GetProjectByIdAsync(new ExistingProjectApiRequest
        {
            ProjectId = taskDal.ProjectId 
        });

        /// Добавляем задачу в спискок задач полученного проекта
        await _projectConnectionService.AddTaskInProjectAsync(new AddTaskInProjectApiRequest 
        { 
            ProjectId = project.Id,
            TaskId = taskDal.Id 
        });

        return _mapper.Map<TaskDTO>(taskDal);
    }

    /// <inheritdoc/>
    public async Task<TaskDTO> DeleteDtoAsync(int taskId)
    {
        var existingTaskDal = await GetExistingTaskAsync(taskId);

        await _unitOfWork.Tasks.DeleteAsync(existingTaskDal);
        return _mapper.Map<TaskDTO>(existingTaskDal);
    }

    /// <inheritdoc/>
    public async Task<TaskDTO> UpdateDtoAsync(TaskDTO taskDTO, int taskId)
    {
        var existingTaskDal = await GetExistingTaskAsync(taskId);

        existingTaskDal = existingTaskDal with
        {
            Name = taskDTO.Name,
            Description = taskDTO.Description,
            ExecutionStatus = taskDTO.ExecutionStatus,
            CreatedDate = taskDTO.CreatedDate,
            LastUpdateDate = taskDTO.LastUpdateDate,
            PerformerIds = taskDTO.PerformerIds,
            CommentIds = taskDTO.CommentIds,
            ProjectId = taskDTO.ProjectId
        };

        foreach (var commentId in taskDTO.CommentIds)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(commentId);
            if (comment != null)
                existingTaskDal.Comments.Add(comment);
        }

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
        return existingTaskDal ?? throw new ValidationException("Task was not found in database", string.Empty);
    }
}
