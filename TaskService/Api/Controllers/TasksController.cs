using Logic.DTO;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers.Task.Requests;
using Api.Controllers.Task.Responses;
using ConnectionLib.ConnectionServices.Interfaces;
using ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления задачами
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly IDtoService<TaskDTO, int> _taskService;

    /// <summary>
    /// Конструктор контроллера задач
    /// </summary>
    /// <param name="taskService">Сервис для работы с задачами</param>
    public TasksController(IDtoService<TaskDTO, int> taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Получает информацию о задаче по идентификатору
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    [HttpGet("{taskId}")]
    [ProducesResponseType<TaskInfoResponse>(200)]
    public async Task<IActionResult> GetInfoAsync([FromRoute] int taskId)
    {
        var task = await _taskService.GetDtoByIdAsync(taskId);
        return Ok(new TaskInfoResponse
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            ExecutionStatus = task.ExecutionStatus,
            CreatedDate = task.CreatedDate,
            LastUpdateDate = task.LastUpdateDate,
            PerformerIds = task.PerformerIds,
            CommentIds = task.CommentIds,
        });
    }

    /// <summary>
    /// Создает новую задачу
    /// </summary>
    /// <param name="request">Запрос на создание задачи</param>
    [HttpPost]
    [ProducesResponseType<CreateTaskResponse>(200)]
    public async Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskDTO request)
    {
        var newTask = await _taskService.CreateDtoAsync(new TaskDTO
        {
            Name = request.Name,
            Description = request.Description,
            ExecutionStatus = request.ExecutionStatus,
            CreatedDate = request.CreatedDate,
            LastUpdateDate = request.LastUpdateDate,
            ProjectId = request.ProjectId,
            PerformerIds = request.PerformerIds,
            CommentIds = request.CommentIds
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
            PerformerIds = newTask.PerformerIds,
            CommentIds = newTask.CommentIds
        });
    }

    /// <summary>
    /// Удаляет задачу по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    [HttpDelete("{taskId}")]
    [ProducesResponseType<DeleteTaskResponse>(200)]
    public async Task<IActionResult> DeleteTaskAsync([FromRoute] int id)
    {
        var deletedTask = await _taskService.DeleteDtoAsync(id);
        return Ok(new DeleteTaskResponse
        {
            Name = deletedTask.Name,
            Description = deletedTask.Description,
            ExecutionStatus = deletedTask.ExecutionStatus,
            CreatedDate = deletedTask.CreatedDate,
            LastUpdateDate = deletedTask.LastUpdateDate,
            PerformerIds = deletedTask.PerformerIds,
            ProjectId = deletedTask.ProjectId,
            CommentIds = deletedTask.CommentIds
        });
    }

    /// <summary>
    /// Обновляет информацию о задаче
    /// </summary>
    /// <param name="request">Запрос на обновление информации о задаче</param>
    /// <param name="taskId">Идентификатор задачи</param>
    [HttpPut("{taskId}")]
    [ProducesResponseType<UpdateTaskResponse>(200)]
    public async Task<IActionResult> UpdateTaskAsync([FromRoute] UpdateTaskDTO request, [FromRoute] int taskId)
    {
        var taskDTO = new TaskDTO
        {
            Name = request.Name,
            Description = request.Description,
            ExecutionStatus = request.ExecutionStatus,
            CreatedDate = request.CreatedDate,
            LastUpdateDate = request.LastUpdateDate,
            PerformerIds = request.PerformerIds,
            CommentIds = request.CommentIds
        };

        var updatedTaskDal = await _taskService.UpdateDtoAsync(taskDTO, taskId);
        return Ok(new UpdateTaskResponse
        {
            Name = updatedTaskDal.Name,
            Description = updatedTaskDal.Description,
            ExecutionStatus = updatedTaskDal.ExecutionStatus,
            CreatedDate = updatedTaskDal.CreatedDate,
            LastUpdateDate = updatedTaskDal.LastUpdateDate,
            PerformerIds = updatedTaskDal.PerformerIds,
            CommentIds = updatedTaskDal.CommentIds
        });
    }
}
