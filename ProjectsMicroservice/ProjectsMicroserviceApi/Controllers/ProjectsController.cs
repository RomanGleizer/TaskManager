using ConnectionLibrary.ConnectionServices.DtoModels.ExistingTaskInProject;
using ConnectionLibrary.ConnectionServices.Interfaces;
using Core.Dal.Base;
using Microsoft.AspNetCore.Mvc;
using ProjectsMicroservice.ProjectsMicroserviceApplication.Interfaces;
using ProjectsMicroservice.ProjectsMicroserviceApplication.ViewModels.ProjectViewModels;
using SemaphoreSynchronizationPrimitiveLibrary.Interfaces;
using UsersMicroservice.UsersMicroserviceLogic.Interfaces;
using UsersMicroservice.UsersMicroserviceLogic.Services;

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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await ExecuteWithSemaphoreAsync("create_project", async () =>
        {
            try
            {
                var result = await projectService.Create(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"При создании пользователя произошла ошибка: {ex.Message}");
            }
        });
    }

    /// <summary>
    ///     Обновляет данные проекта
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="model">Модель обновления проекта</param>
    /// <returns>Данные об обновленном проекте</returns>
    [HttpPut("{projectId:guid}")]
    [ProducesResponseType<ProjectViewModel>(200)]
    public async Task<IActionResult> UpdateProjectAsync(
        [FromRoute] Guid projectId,
        [FromBody] UpdateProjectViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await ExecuteWithSemaphoreAsync("update_project", async () =>
        {
            try
            {
                var result = await projectService.Update(projectId, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"При создании пользователя произошла ошибка: {ex.Message}");
            }
        });
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
        return await ExecuteWithSemaphoreAsync("delete_project", async () =>
        {
            try
            {
                var result = await projectService.Delete(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"При создании пользователя произошла ошибка: {ex.Message}");
            }
        });
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

    [HttpPost("{projectId:guid}/tasks/{taskId:guid}")]
    public async Task<IActionResult> AddTaskInProject([FromRoute] Guid projectId, [FromRoute] Guid taskId)
    {
        await addProjectIdToProjectIdList.AddNewTaskIdInProjectIdList(projectId, taskId);
        return Ok();
    }

    private async Task<IActionResult> ExecuteWithSemaphoreAsync(string operationName, Func<Task<IActionResult>> action)
    {
        var semaphoreAcquired = await semaphore.AcquireAsync(operationName);
        if (!semaphoreAcquired)
            return StatusCode(429, $"Превышен лимит запросов на операцию {operationName}");

        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Во время операции {operationName} произошла ошибка: {ex.Message}");
        }
        finally
        {
            await semaphore.ReleaseAsync(operationName);
        }
    }
}