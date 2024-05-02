using Core.Dal.Base;
using Microsoft.AspNetCore.Mvc;
using ProjectsMicroservice.ProjectsMicroserviceApplication.Interfaces;
using ProjectsMicroservice.ProjectsMicroserviceApplication.Services;
using SemaphoreSynchronizationPrimitiveLibrary.Interfaces;
using TasksMicroservice.TasksMicroserviceApi.Controllers.Task.Requests;
using TasksMicroservice.TasksMicroserviceApi.Controllers.Task.Responses;
using TasksMicroservice.TasksMicroserviceLogic.Dto;

namespace TasksMicroservice.TasksMicroserviceApi.Controllers;

/// <summary>
///     Контроллер для управления задачами
/// </summary>
/// <remarks>
///     Конструктор контроллера задач
/// </remarks>
/// <param name="taskService">Сервис для работы с задачами</param>
[Route("api/[controller]")]
[ApiController]
public class TasksController(IDtoService<TaskDto, Guid> taskService, IDistributedSemaphore semaphore) : ControllerBase
{
    /// <summary>
    ///     Получает информацию о задаче по идентификатору
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    [HttpGet("{taskId:guid}")]
    [ProducesResponseType<TaskInfoResponse>(200)]
    public async Task<IActionResult> GetInfoAsync([FromRoute] Guid taskId)
    {
        var task = await taskService.GetDtoByIdAsync(taskId);
        return Ok(new TaskInfoResponse
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            ExecutionStatus = task.ExecutionStatus,
            CreatedDate = task.CreatedDate,
            LastUpdateDate = task.LastUpdateDate,
            PerformerIds = task.PerformerIds
        });
    }

    /// <summary>
    ///     Создает новую задачу
    /// </summary>
    /// <param name="request">Запрос на создание задачи</param>
    [HttpPost]
    [ProducesResponseType<CreateTaskResponse>(200)]
    public async Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await ExecuteWithSemaphoreAsync("create_task", async () =>
        {
            try
            {
                var newTask = await taskService.CreateDtoAsync(new TaskDto
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                    ExecutionStatus = request.ExecutionStatus,
                    CreatedDate = request.CreatedDate,
                    LastUpdateDate = request.LastUpdateDate,
                    ProjectId = request.ProjectId,
                    PerformerIds = request.PerformerIds
                });
                return Ok(new CreateTaskResponse
                {
                    Id = newTask.Id,
                    Name = newTask.Name,
                    Description = newTask.Description,
                    ExecutionStatus = newTask.ExecutionStatus,
                    CreatedDate = newTask.CreatedDate,
                    LastUpdateDate = newTask.LastUpdateDate,
                    ProjectId = newTask.ProjectId,
                    PerformerIds = newTask.PerformerIds
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"При создании пользователя произошла ошибка: {ex.Message}");
            }
        });
    }

    /// <summary>
    ///     Удаляет задачу по идентификатору
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    [HttpDelete("{taskId:guid}")]
    [ProducesResponseType<DeleteTaskResponse>(200)]
    public async Task<IActionResult> DeleteTaskAsync([FromRoute] Guid taskId)
    {
        return await ExecuteWithSemaphoreAsync("delete_task", async () =>
        {
            try
            {
                var deletedTask = await taskService.DeleteDtoAsync(taskId);
                return Ok(new DeleteTaskResponse
                {
                    Name = deletedTask.Name,
                    Description = deletedTask.Description,
                    ExecutionStatus = deletedTask.ExecutionStatus,
                    CreatedDate = deletedTask.CreatedDate,
                    LastUpdateDate = deletedTask.LastUpdateDate,
                    PerformerIds = deletedTask.PerformerIds,
                    ProjectId = deletedTask.ProjectId
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"При создании пользователя произошла ошибка: {ex.Message}");
            }
        });
    }

    /// <summary>
    ///     Обновляет информацию о задаче
    /// </summary>
    /// <param name="request">Запрос на обновление информации о задаче</param>
    /// <param name="taskId">Идентификатор задачи</param>
    [HttpPut("{taskId:guid}")]
    [ProducesResponseType<UpdateTaskResponse>(200)]
    public async Task<IActionResult> UpdateTaskAsync([FromRoute] UpdateTaskDto request, [FromRoute] Guid taskId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await ExecuteWithSemaphoreAsync("update_task", async () =>
        {
            try
            {
                var taskDto = new TaskDto
                {
                    Id = taskId,
                    Name = request.Name,
                    Description = request.Description,
                    ExecutionStatus = request.ExecutionStatus,
                    CreatedDate = request.CreatedDate,
                    LastUpdateDate = request.LastUpdateDate,
                    PerformerIds = request.PerformerIds,
                    ProjectId = request.ProjectId
                };

                var updatedTaskDal = await taskService.UpdateDtoAsync(taskDto, taskId);
                return Ok(new UpdateTaskResponse
                {
                    Name = updatedTaskDal.Name,
                    Description = updatedTaskDal.Description,
                    ExecutionStatus = updatedTaskDal.ExecutionStatus,
                    CreatedDate = updatedTaskDal.CreatedDate,
                    LastUpdateDate = updatedTaskDal.LastUpdateDate,
                    PerformerIds = updatedTaskDal.PerformerIds
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"При создании пользователя произошла ошибка: {ex.Message}");
            }
        });
    }

    /// <summary>
    ///     Получает список всех задач из определнного проекта
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    [HttpGet("projects/{projectId:guid}")]
    public async Task<IActionResult> GetAllTasksFromCertainProject([FromRoute] Guid projectId)
    {
        var result = await taskService.GetAllDtosAsync();
        var tasks = result
            .Where(t => t.ProjectId == projectId)
            .Select(t => t.Id)
            .ToList();

        return Ok(tasks);
    }

    private async Task<IActionResult> ExecuteWithSemaphoreAsync(string operationName, Func<Task<IActionResult>> action)
    {
        var semaphoreAcquired = await semaphore.AcquireAsync(operationName);
        if (!semaphoreAcquired)
            return StatusCode(429, $"Превышен лимит запросов на операцию {operationName}");

        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Во время операции {operationName} произошла ошибка: {ex.Message}");
        }
        finally
        {
            await semaphore.ReleaseAsync(operationName);
        }
    }
}