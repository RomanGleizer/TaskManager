using Api.Controllers.Task.Requests;
using Api.Controllers.Task.Responses;
using Logic.DTO;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("{taskId}")]
    [ProducesResponseType<TaskInfoResponse>(200)]
    public async Task<IActionResult> GetInfoAsync([FromQuery] int taskId)
    {
        var task = await _taskService.GetTaskByIdAsync(taskId);
        return Ok(new TaskInfoResponse
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            ExecutionStatus = task.ExecutionStatus,
            CreatedDate = task.CreatedDate,
            LastUpdateDate = task.LastUpdateDate,
            PerformerIds = task.PerformerIds,
            StageDirectorId = task.StageDirectorId
        });
    }

    [HttpPost]
    [ProducesResponseType<CreateTaskResponse>(200)]
    public async Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskRequest request)
    {
        var newTaskDal = await _taskService.CreateTaskAsync(new TaskDTO
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
            Id = newTaskDal.Id,
            Name = newTaskDal.Name,
            Description = newTaskDal.Description,
            ExecutionStatus = newTaskDal.ExecutionStatus,
            CreatedDate = newTaskDal.CreatedDate,
            LastUpdateDate = newTaskDal.LastUpdateDate,
            ProjectId = newTaskDal.ProjectId,
            PerformerIds = newTaskDal.PerformerIds
        });
    }

    [HttpDelete("{taskId}")]
    [ProducesResponseType<DeleteTaskResponse>(200)]
    public async Task<IActionResult> DeleteTaskAsync([FromQuery] int id)
    {
        var deletedTask = await _taskService.DeleteTaskAsync(id);
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

    [HttpPut("{taskId}")]
    [ProducesResponseType<UpdateTaskResponse>(200)]
    public async Task<IActionResult> UpdateTaskAsync([FromBody] UpdateTaskRequest request, [FromQuery] int taskId)
    {
        var taskDTO = new TaskDTO
        {
            Name = request.Name,
            Description = request.Description,
            ExecutionStatus = request.ExecutionStatus,
            CreatedDate = request.CreatedDate,
            LastUpdateDate = request.LastUpdateDate,
            PerformerIds = request.PerformerIds
        };

        var updatedTaskDal = await _taskService.UpdateTaskAsync(taskDTO, taskId);
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
}
