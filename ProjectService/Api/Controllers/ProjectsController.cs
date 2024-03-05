﻿using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.ProjectViewModels;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления проектами
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Получает проект по его идентификатору
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <returns>Данные о проекте</returns>
    [HttpGet("{projectId}")]
    [ProducesResponseType<ProjectViewModel>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectById([FromRoute] int projectId)
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
    [ProducesResponseType<ProjectViewModel>(StatusCodes.Status200OK)]
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
    [ProducesResponseType<ProjectViewModel>(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateProjectAsync([FromRoute] int projectId, [FromBody] UpdateProjectViewModel model)
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
    [ProducesResponseType<ProjectViewModel>(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteProjectAsync([FromRoute] int projectId)
    {
        var deletedProjectViewModel = await _projectService.Delete(projectId);
        return Ok(deletedProjectViewModel);
    }
}
