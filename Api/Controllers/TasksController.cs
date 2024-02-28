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
    private readonly IDtoService<TaskDTO, int> _taskService;

    public TasksController(IDtoService<TaskDTO, int> taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("{taskId}")]
    [ProducesResponseType<TaskInfoResponse>(200)]
    public async Task<IActionResult> GetInfoAsync([FromQuery] int taskId)
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

    [HttpPost]
    [ProducesResponseType<CreateTaskResponse>(200)]
    public async Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskRequest request)
    {
        var newTaskDal = await _taskService.CreateDtoAsync(new TaskDTO
        {
            Id = request.Id,
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
            Id = newTaskDal.Id,
            Name = newTaskDal.Name,
            Description = newTaskDal.Description,
            ExecutionStatus = newTaskDal.ExecutionStatus,
            CreatedDate = newTaskDal.CreatedDate,
            LastUpdateDate = newTaskDal.LastUpdateDate,
            ProjectId = newTaskDal.ProjectId,
            PerformerIds = newTaskDal.PerformerIds,
            CommentIds = newTaskDal.CommentIds
        });
    }

    [HttpDelete("{taskId}")]
    [ProducesResponseType<DeleteTaskResponse>(200)]
    public async Task<IActionResult> DeleteTaskAsync([FromQuery] int id)
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
