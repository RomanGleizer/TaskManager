using ConnectionLibrary.ConnectionServices.DtoModels.ExistingTaskInProject;
using ConnectionLibrary.ConnectionServices.Interfaces;
using Core.Dal.Base;
using Microsoft.AspNetCore.Mvc;
using ProjectsMicroservice.ProjectsMicroserviceApplication.Interfaces;
using ProjectsMicroservice.ProjectsMicroserviceApplication.ViewModels.ProjectViewModels;
using SemaphoreSynchronizationPrimitiveLibrary.Interfaces;

namespace ProjectsMicroservice.ProjectsMicroserviceApi.Controllers;

/// <summary>
///     Контроллер для управления проектами
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProjectsController(
    IProjectService projectService,
    ITaskConnectionService taskConnectionService,
    IAddTaskIdToProjectTaskIdList addProjectIdToProjectIdList,
    IDistributedSemaphore semaphore) : ControllerBase
{
    /// <summary>
    ///     Получает проект по его идентификатору
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <returns>Данные о проекте</returns>
    [HttpGet("{projectId:guid}")]
    [ProducesResponseType<ProjectViewModel>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectById([FromRoute] Guid projectId)
    {
        var existingProjectViewModel = await projectService.GetById(projectId);
        return Ok(existingProjectViewModel);
    }

    /// <summary>
    ///     Создает новый проект
    /// </summary>
    /// <param name="model">Модель создания проекта</param>
    /// <returns>Данные о созданном проекте</returns>
    [HttpPost]
    [ProducesResponseType<ProjectViewModel>(200)]
    public async Task<IActionResult> CreateProjectAsync([FromBody] CreateProjectViewModel model)
    {
        var semaphoreAcquired = await semaphore.AcquireAsync("create_project");
        if (!semaphoreAcquired)
            return StatusCode(429, "Превышен лимит запросов на создание проекта");

        try
        {
            var createdProjectViewModel = await projectService.Create(model);
            return Ok(createdProjectViewModel);
        }
        catch (Exception ex)
        {
            throw new Exception($"При создании проекта произошла ошибка: {ex.Message}");
        }
        finally
        {
            semaphore.Release("create_project");
        }
    }

    /// <summary>
    ///     Обновляет данные проекта
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="model">Модель обновления проекта</param>
    /// <returns>Данные об обновленном проекте</returns>
    [HttpPut("{projectId:guid}")]
    [ProducesResponseType<ProjectViewModel>(200)]
    public async Task<IActionResult> UpdateProjectAsync([FromRoute] Guid projectId,
        [FromBody] UpdateProjectViewModel model)
    {
        var semaphoreAcquired = await semaphore.AcquireAsync("update_project");
        if (!semaphoreAcquired)
            return StatusCode(429, $"Превышен лимит запросов на обновление проекта с id {projectId}");

        try
        {
            var updatedProjectViewModel = await projectService.Update(projectId, model);
            return Ok(updatedProjectViewModel);
        }
        catch (Exception ex)
        {
            throw new Exception($"При обновлении проекта произошла ошибка: {ex.Message}");
        }
        finally
        {
            semaphore.Release("update_project");
        }
    }

    /// <summary>
    ///     Удаляет проект
    /// </summary>
    /// <param name="projectId">Идентификатор проекта для удаления</param>
    /// <returns>Данные об удаленном проекте</returns>
    [HttpDelete("{projectId:guid}")]
    [ProducesResponseType<ProjectViewModel>(200)]
    public async Task<IActionResult> DeleteProjectAsync([FromRoute] Guid projectId)
    {
        var semaphoreAcquired = await semaphore.AcquireAsync("delete_project");
        if (!semaphoreAcquired)
            return StatusCode(429, $"Превышен лимит запросов на удаление проекта с id {projectId}");

        try
        {
            var deletedProjectViewModel = await projectService.Delete(projectId);
            return Ok(deletedProjectViewModel);
        }
        catch (Exception ex)
        {
            throw new Exception($"При удалении проекта произошла ошибка: {ex.Message}");
        }
        finally
        {
            semaphore.Release("delete_project");
        }
    }

    /// <summary>
    ///     Получает задачу проекта по её идентификатору
    /// </summary>
    /// ///
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <returns>Ответ с информацией о задаче</returns>
    [HttpGet("{projectId:guid}/tasks/{taskId:guid}")]
    [ProducesResponseType<ExistingTaskInProjectResponse>(200)]
    public async Task<IActionResult> GetExistingTask([FromRoute] Guid projectId, [FromRoute] Guid taskId)
    {
        var existingTask = await taskConnectionService.GetExistingTaskAsync(new ExistingTaskInProjectRequest
        {
            ProjectId = projectId,
            TaskId = taskId
        });

        if (existingTask.TaskIds.Contains(taskId))
            return Ok(new { Contains = true });
        return NotFound($"Задача с id {taskId} не была найдена в БД");
    }

    [HttpGet("{projectId:guid}/tasks/{taskId:guid}")]
    public async Task<IActionResult> AddTaskInProject([FromRoute] Guid projectId, [FromRoute] Guid taskId)
    {
        await addProjectIdToProjectIdList.AddNewTaskIdInProjectIdList(projectId, taskId);
        return Ok();
    }
}