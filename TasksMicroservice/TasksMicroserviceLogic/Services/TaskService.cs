using AutoMapper;
using Core.Dal.Base;
using MassTransit;
using TasksMicroservice.TasksMicroserviceDal.Entities;
using TasksMicroservice.TasksMicroserviceDal.Interfaces;
using TasksMicroservice.TasksMicroserviceLogic.Dto;
using TasksMicroservice.TasksMicroserviceLogic.TaskSaga;

namespace TasksMicroservice.TasksMicroserviceLogic.Services;

/// <summary>
///     Сервис для работы с задачами
/// </summary>
/// <remarks>
///     Конструктор сервиса задач
/// </remarks>
/// <param name="unitOfWork">Единица работы с данными</param>
/// <param name="mapper">Маппер для преобразования между объектами</param>
public class TaskService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IBus bus)
    : IDtoService<TaskDto, Guid>
{
    /// <inheritdoc />
    public async Task<IList<TaskDto>> GetAllDtosAsync()
    {
        var allTaskDals = await unitOfWork.Tasks.GetAllAsync();
        return mapper.Map<IList<TaskDal>, IList<TaskDto>>(allTaskDals);
    }

    /// <inheritdoc />
    public async Task<TaskDto> GetDtoByIdAsync(Guid taskId)
    {
        var taskDal = await GetExistingTaskAsync(taskId);
        return mapper.Map<TaskDto>(taskDal);
    }

    /// <inheritdoc />
    public async Task<TaskDto> CreateDtoAsync(TaskDto taskDto)
    {
        var taskDal = mapper.Map<TaskDal>(taskDto);

        var message = new CreateTaskSagaRequest(taskDal.ProjectId, taskDal.Id);
        await bus.Publish(message);

        await unitOfWork.Tasks.CreateAsync(taskDal);
        return mapper.Map<TaskDto>(taskDal);
    }

    /// <inheritdoc />
    public async Task<TaskDto> DeleteDtoAsync(Guid taskId)
    {
        var existingTaskDal = await GetExistingTaskAsync(taskId);

        await unitOfWork.Tasks.DeleteAsync(existingTaskDal);
        return mapper.Map<TaskDto>(existingTaskDal);
    }

    /// <inheritdoc />
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
    ///     Асинхронное получение задачи по Id
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    private async Task<TaskDal> GetExistingTaskAsync(Guid taskId)
    {
        var existingTaskDal = await unitOfWork.Tasks.GetByIdAsync(taskId);
        return existingTaskDal ?? throw new Exception("Задача не найдена в бд");
    }
}