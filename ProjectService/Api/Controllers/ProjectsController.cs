using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.ProjectViewModels;

namespace ProjectApi.Controllers;

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
    [ProducesResponseType<ProjectViewModel>(200)]
    public async Task<IActionResult> GetProjectById([FromRoute] int projectId)
    {
        var existingProject = await _projectService.GetById(projectId);
        return Ok(existingProject);
    }

    [HttpPost]
    [ProducesResponseType<ProjectViewModel>(200)]
    public async Task<IActionResult> CreateProjectAsync([FromBody] CreateProjectViewModel model)
    {
        var createdProjectId = await _projectService.Create(model);
        return Ok(createdProjectId);
    }

    [HttpPut("{projectId}")]
    [ProducesResponseType<ProjectViewModel>(200)]
    public async Task<IActionResult> UpdateProjectAsync([FromQuery] int projectId, [FromBody] UpdateProjectViewModel model)
    {
        var updatedProjectId = await _projectService.Update(projectId, model);
        return Ok(updatedProjectId);
    }

    [HttpDelete("{projectId}")]
    [ProducesResponseType<ProjectViewModel>(200)]
    public async Task<IActionResult> DeleteProjectAsync([FromQuery] int projectId)
    {
        var deletedProjectId = await _projectService.Delete(projectId);
        return Ok(deletedProjectId);
    }
}