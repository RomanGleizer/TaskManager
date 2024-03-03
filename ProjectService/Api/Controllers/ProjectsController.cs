using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using ProjectApi.Controllers.Project_Requests;
using Services.Interfaces.ProjectInterfaces;
using Services.Interfaces.Project_Interfaces;
using Domain.Updated;

namespace ProjectApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IFindProject _findProject;
    private readonly ICreateProject _createProject;
    private readonly IUpdateProject _updateProject;
    private readonly IDeleteProject _deleteProject;

    public ProjectsController(IFindProject findProject, ICreateProject createProject, IUpdateProject updateProject, IDeleteProject deleteProject)
    {
        _findProject = findProject;
        _createProject = createProject;
        _updateProject = updateProject;
        _deleteProject = deleteProject;
    }

    [HttpGet("{projectId}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetProjectById([FromRoute] int projectId)
    {
        var existingProject = await _findProject.GetProjectByIdAsync(projectId);
        return Ok(existingProject);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> CreateProjectAsync([FromBody] CreateProjectRequest request)
    {
        var createdProjectId = await _createProject.CreateProjectAsync(new Project
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            CreationDate = DateTime.Now,
            LastUpdatedDate = DateTime.Now,
            Members = new List<Member>(),
            MemberIds = new List<string>()
        });

        return Ok(createdProjectId);
    }

    [HttpPut("{projectId}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> UpdateProjectAsync([FromQuery] int projectId, [FromBody] UpdateProjectRequest request)
    {
        var projectData = new UpdateProjectData
        {
            Name = request.Name,
            Description = request.Description,
            LastUpdatedDate = request.LastUpdatedDate,
            MemberIds = request.MemberIds
        };

        var updatedProjectId = await _updateProject.UpdateProjectAsync(projectId, projectData);
        return Ok(updatedProjectId);
    }

    [HttpDelete("{projectId}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DeleteProjectAsync([FromQuery] int projectId)
    {
        var deletedProjectId = await _deleteProject.DeleteProjectAsync(projectId);
        return Ok(deletedProjectId);
    }
}
