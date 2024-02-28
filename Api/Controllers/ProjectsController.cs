using Api.Controllers.Project.Requests;
using Api.Controllers.Project.Responses;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Logic.DTO;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("{projectId}")]
    [ProducesResponseType<ProjectInfoResponse>(200)]
    public async Task<IActionResult> GetInfoAsync([FromQuery] int projectId)
    {
        var project = await _projectService.GetProjectByIdAsync(projectId);
        return Ok(new ProjectInfoResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreationDate = project.CreationDate,
            LastModifidedDate = project.LastModifidedDate,
            ParticipantIds = project.ParticipantIds,
            TaskIds = project.TaskIds
        });
    }

    [HttpPost]
    [ProducesResponseType<CreateProjectResponse>(200)]
    public async Task<IActionResult> CreateProjectAsync([FromBody] CreateProjectRequest request)
    {
        var newProjectDal = await _projectService.CreateProjectAsync(new ProjectDTO
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            CreationDate = DateTime.Now,
            LastModifidedDate = default,
            ParticipantIds = new List<string>(),
            TaskIds = new List<int>()
        });

        return Ok(new CreateProjectResponse
        {
            Id = newProjectDal.Id,
            Name = newProjectDal.Name,
            Description = newProjectDal.Description,
            CreationDate = DateTime.Now,
            LastModifidedDate = default,
            ParticipantIds = new List<string>(),
            TaskIds = new List<int>()
        });
    }

    [HttpDelete("{projectId}")]
    [ProducesResponseType<DeleteProjectResponse>(200)]
    public async Task<IActionResult> DeleteProjectAsync([FromQuery] int projectId)
    {
        var deletedProject = await _projectService.DeleteProjectAsync(projectId);
        return Ok(new DeleteProjectResponse
        {
            Id = deletedProject.Id,
            Name = deletedProject.Name,
            Description = deletedProject.Description,
            CreationDate = deletedProject.CreationDate,
            LastModifidedDate = deletedProject.LastModifidedDate,
            ParticipantIds = deletedProject.ParticipantIds,
            TaskIds = deletedProject.TaskIds
        });
    }

    [HttpPut("{projectId}")]
    [ProducesResponseType<UpdateProjectResponse>(200)]
    public async Task<IActionResult> UpdateProjectAsync([FromBody] UpdateProjectRequest request, [FromQuery] int projectId)
    {
        var projectDTO = new ProjectDTO
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            CreationDate = request.CreationDate,
            LastModifidedDate = request.LastModifidedDate,
            ParticipantIds = request.ParticipantIds,
            TaskIds = request.TaskIds
        };

        var updatedProject = await _projectService.UpdateProjectAsync(projectDTO, projectId);
        return Ok(new UpdateProjectResponse
        {
            Id = updatedProject.Id,
            Name = updatedProject.Name,
            Description = updatedProject.Description,
            CreationDate = updatedProject.CreationDate,
            LastModifidedDate = updatedProject.LastModifidedDate,
            ParticipantIds = updatedProject.ParticipantIds,
            TaskIds = updatedProject.TaskIds
        });
    }

    // Дописать ресты обновления, удаления, добавления (задачи, нового участника)
}
