using AutoMapper;
using ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;
using ConnectionLib.ConnectionServices.DtoModels.ProjectById;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.Dal.Base;
using Core.Exceptions;
using Dal.Entities;
using Dal.Interfaces;
using Logic.DTO;

namespace Logic.Services;

/// <summary>
/// Сервис для работы с задачами
/// </summary>
/// <remarks>
/// Конструктор сервиса задач
/// </remarks>
/// <param name="unitOfWork">Единица работы с данными</param>
/// <param name="mapper">Маппер для преобразования между объектами</param>
public class TaskService(
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    IProjectConnectionService projectConnectionService) 
    : IDtoService<TaskDto, Guid>
{
    /// <inheritdoc/>
    public async Task<IList<TaskDto>> GetAllDtosAsync()
    {
        var allTaskDals = await unitOfWork.Tasks.GetAllAsync();
        return mapper.Map<IList<TaskDal>, IList<TaskDto>>(allTaskDals);
    }

    /// <inheritdoc/>
    public async Task<TaskDto> GetDtoByIdAsync(Guid taskId)
    {
        var taskDal = await GetExistingTaskAsync(taskId);
        return mapper.Map<TaskDto>(taskDal);
    }

    /// <inheritdoc/>
    public async Task<TaskDto> CreateDtoAsync(TaskDto taskDto)
    {
        var taskDal = mapper.Map<TaskDal>(taskDto);
        await unitOfWork.Tasks.CreateAsync(taskDal);

        var project = await projectConnectionService.GetProjectByIdAsync(
                          new IsProjectExistsRequest
                          {
                              ProjectId = taskDal.ProjectId
                          })
                      ?? throw new ValidationException("Проект не найден в БД", string.Empty);

        await projectConnectionService.AddTaskIdInProjectTaskIdsAsync(new AddTaskIdInProjectTaskIdsRequest
        {
            ProjectId = project.Id,
            TaskId = taskDal.Id
        });

        return mapper.Map<TaskDto>(taskDal);
    }

    /// <inheritdoc/>
    public async Task<TaskDto> DeleteDtoAsync(Guid taskId)
    {
        var existingTaskDal = await GetExistingTaskAsync(taskId);

        await unitOfWork.Tasks.DeleteAsync(existingTaskDal);
        return mapper.Map<TaskDto>(existingTaskDal);
    }

    /// <inheritdoc/>
    public async Task<TaskDto> UpdateDtoAsync(TaskDto taskDto, Guid taskId)
    {
        var existingTaskDal = await GetExistingTaskAsync(taskId);

        existingTaskDal = existingTaskDal with
        {
            Name = taskDto.Name,
            Description = taskDto.Description,
            ExecutionStatus = taskDto.ExecutionStatus,
            CreatedDate = taskDto.CreatedDate,
            LastUpdateDate = taskDto.LastUpdateDate,
            PerformerIds = taskDto.PerformerIds,
            ProjectId = taskDto.ProjectId
        };

        await unitOfWork.Tasks.UpdateAsync(existingTaskDal);
        return mapper.Map<TaskDto>(existingTaskDal);
    }

    /// <summary>
    /// Асинхронное получение задачи по Id
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    private async Task<TaskDal> GetExistingTaskAsync(Guid taskId)
    {
        var existingTaskDal = await unitOfWork.Tasks.GetByIdAsync(taskId);
        return existingTaskDal ?? throw new ValidationException("Задача не найдена в бд", string.Empty);
    }
}
