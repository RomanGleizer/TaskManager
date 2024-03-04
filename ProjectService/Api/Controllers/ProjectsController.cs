using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.ProjectViewModels;

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
    [ProducesResponseType<ProjectViewModel>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectById([FromRoute] int projectId)
    {
        var existingProjectViewModel = await _projectService.GetById(projectId);
        return Ok(existingProjectViewModel);
    }

    [HttpPost]
    [ProducesResponseType<ProjectViewModel>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateProjectAsync([FromBody] CreateProjectViewModel model)
    {
        var createdProjectViewModel = await _projectService.Create(model);
        return Ok(createdProjectViewModel);
    }

    [HttpPut("{projectId}")]
    [ProducesResponseType<ProjectViewModel>(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateProjectAsync([FromRoute] int projectId, [FromBody] UpdateProjectViewModel model)
    {
        var updatedProjectViewModel = await _projectService.Update(projectId, model);
        return Ok(updatedProjectViewModel);
    }

    [HttpDelete("{projectId}")]
    [ProducesResponseType<ProjectViewModel>(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteProjectAsync([FromRoute] int projectId)
    {
        var deletedProjectViewModel = await _projectService.Delete(projectId);
        return Ok(deletedProjectViewModel);
    }
}