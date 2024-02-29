using Logic.DTO;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers.Project.Requests;
using Api.Controllers.Project.Responses;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления проектами
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IDtoService<ProjectDTO, int> _projectService;
    private readonly IProjectDtoManager _projectDtoManager;

    /// <summary>
    /// Конструктор контроллера проектов
    /// </summary>
    /// <param name="projectService">Сервис для работы с проектами</param>
    /// <param name="projectDtoManager">Менеджер DTO для проектов</param>
    public ProjectsController(IDtoService<ProjectDTO, int> projectService, IProjectDtoManager projectDtoManager)
    {
        _projectService = projectService;
        _projectDtoManager = projectDtoManager;
    }

    /// <summary>
    /// Получает информацию о проекте по идентификатору
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    [HttpGet("{projectId}")]
    [ProducesResponseType<ProjectInfoResponse>(200)]
    public async Task<IActionResult> GetInfoAsync([FromQuery] int projectId)
    {
        var project = await _projectService.GetDtoByIdAsync(projectId);
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

    /// <summary>
    /// Создает новый проект
    /// </summary>
    /// <param name="request">Запрос на создание проекта</param>
    [HttpPost]
    [ProducesResponseType<CreateProjectResponse>(200)]
    public async Task<IActionResult> CreateProjectAsync([FromBody] CreateProjectRequest request)
    {
        var newProjectDal = await _projectService.CreateDtoAsync(new ProjectDTO
        {
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

    /// <summary>
    /// Удаляет проект по идентификатору
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    [HttpDelete("{projectId}")]
    [ProducesResponseType<DeleteProjectResponse>(200)]
    public async Task<IActionResult> DeleteProjectAsync([FromQuery] int projectId)
    {
        var deletedProject = await _projectService.DeleteDtoAsync(projectId);
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

    /// <summary>
    /// Обновляет информацию о проекте
    /// </summary>
    /// <param name="request">Запрос на обновление информации о проекте</param>
    /// <param name="projectId">Идентификатор проекта</param>
    [HttpPut("{projectId}")]
    [ProducesResponseType<UpdateProjectResponse>(200)]
    public async Task<IActionResult> UpdateProjectAsync([FromBody] UpdateProjectRequest request, [FromQuery] int projectId)
    {
        var projectDTO = new ProjectDTO
        {
            Name = request.Name,
            Description = request.Description,
            CreationDate = request.CreationDate,
            LastModifidedDate = request.LastModifidedDate,
            ParticipantIds = request.ParticipantIds,
            TaskIds = request.TaskIds
        };

        var updatedProject = await _projectService.UpdateDtoAsync(projectDTO, projectId);
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

    /// <summary>
    /// Добавляет нового участника к проекту
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="participantId">Идентификатор участника</param>
    [HttpPost("add/participant/{projectId}/{participantId}")]
    [ProducesResponseType<UpdateProjectResponse>(200)]
    public async Task<IActionResult> AddNewParticipantAsync([FromQuery] int projectId, [FromQuery] string participantId)
    {
        var updatedProject = await _projectDtoManager.AddParticipantAsync(projectId, participantId);
        return Ok(new UpdateProjectResponse { ParticipantIds = updatedProject.ParticipantIds });
    }
}
