using ConnectionLib.ConnectionServices.DtoModels.TaskById;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.Dal.Base;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.ProjectViewModels;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления проектами
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProjectsController(
    IProjectService projectService,
    ITaskConnectionService taskConnectionService,
    IAddTaskIdToProjectTaskIdList addProjectIdToProjectIdList) : ControllerBase
{
    private readonly IProjectService _projectService = projectService;
    private readonly ITaskConnectionService _taskConnectionService = taskConnectionService;
    private readonly IAddTaskIdToProjectTaskIdList _addTaskIdToProjectIdList = addProjectIdToProjectIdList;

    /// <summary>
    /// Получает проект по его идентификатору
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <returns>Данные о проекте</returns>
    [HttpGet("{projectId}")]
    [ProducesResponseType<ProjectViewModel>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectById([FromRoute] Guid projectId)
    {
        var existingProjectViewModel = await _projectService.GetById(projectId);
        return Ok(existingProjectViewModel);
    }

    /// <summary>
    /// Создает новый проект
    /// </summary>
    /// <param name="model">Модель создания проекта</param>
    /// <returns>Данные о созданном проекте</returns>
    [HttpPost]
    [ProducesResponseType<ProjectViewModel>(200)]
    public async Task<IActionResult> CreateProjectAsync([FromBody] CreateProjectViewModel model)
    {
        var createdProjectViewModel = await _projectService.Create(model);
        return Ok(createdProjectViewModel);
    }

    /// <summary>
    /// Обновляет данные проекта
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="model">Модель обновления проекта</param>
    /// <returns>Данные о обновленном проекте</returns>
    [HttpPut("{projectId}")]
    [ProducesResponseType<ProjectViewModel>(200)]
    public async Task<IActionResult> UpdateProjectAsync([FromRoute] Guid projectId, [FromBody] UpdateProjectViewModel model)
    {
        var updatedProjectViewModel = await _projectService.Update(projectId, model);
        return Ok(updatedProjectViewModel);
    }

    /// <summary>
    /// Удаляет проект
    /// </summary>
    /// <param name="projectId">Идентификатор проекта для удаления</param>
    /// <returns>Данные об удаленном проекте</returns>
    [HttpDelete("{projectId}")]
    [ProducesResponseType<ProjectViewModel>(200)]
    public async Task<IActionResult> DeleteProjectAsync([FromRoute] Guid projectId)
    {
        var deletedProjectViewModel = await _projectService.Delete(projectId);
        return Ok(deletedProjectViewModel);
    }

    /// <summary>
    /// Получает задачу проекта по её идентификатору
    /// </summary>
    /// /// <param name="taskId">Идентификатор проекта</param>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <returns>Ответ с информацией о задаче</returns>
    [HttpGet("{projectId}/tasks/{taskId}")]
    [ProducesResponseType<ExistingTaskInProjectResponse>(200)]
    public async Task<IActionResult> GetExistingTask([FromRoute] Guid projectId, [FromRoute] Guid taskId)
    {
        var existingTask = await _taskConnectionService.GetExistingTaskAsync(new ExistingTaskInProjectRequest
        {
            ProjectId = projectId,
            TaskId = taskId
        });

        if (existingTask.TaskIds.Contains(taskId))
            return Ok(new { Contains = true });
        else
            return NotFound("The task was not found in database");
    }

    [HttpPost("{projectId}/tasks/{taskId}")]
    public async Task<IActionResult> AddTaskInProject([FromRoute] Guid projectId, [FromRoute] Guid taskId)
    {
        await _addTaskIdToProjectIdList.AddNewTaskIdInProjectIdList(projectId, taskId);
        return Ok();
    }
}