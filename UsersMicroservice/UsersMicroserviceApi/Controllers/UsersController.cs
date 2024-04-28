﻿using Core.Dal.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SemaphoreSynchronizationPrimitiveLibrary.Interfaces;
using UsersMicroservice.UsersMicroserviceLogic.Dto.User;
using UsersMicroservice.UsersMicroserviceLogic.Interfaces;

namespace UsersMicroservice.UsersMicroserviceApi.Controllers;

/// <summary>
///     Контроллер для управления операциями, связанными с пользователями
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController(
    IUserService userService,
    IAddProjectIdToUserProjectIdList addProjectId,
    IDistributedSemaphore semaphore)
    : ControllerBase
{
    /// <summary>
    ///     Получает всех пользователей из базы данных
    /// </summary>
    /// <returns>Результат действия, содержащий коллекцию пользователей</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<UserDto>>(200)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    ///     Получает пользователя по его уникальному идентификатору
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя для получения</param>
    /// <returns>Результат действия, содержащий информацию о пользователе, если найден, в противном случае возвращает NotFound</returns>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType<UserDto>(200)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
    {
        var result = await userService.GetUserByIdAsync(userId);
        return Ok(result);
    }

    /// <summary>
    ///     Создает нового пользователя
    /// </summary>
    /// <param name="dto">DTO, представляющий пользователя, который должен быть создан</param>
    /// <returns>Результат действия, указывающий на успешность или неудачу операции создания</returns>
    [HttpPost]
    [ProducesResponseType<IdentityResult>(200)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var semaphoreAcquired = await semaphore.AcquireAsync("create_user");
        if (!semaphoreAcquired)
            return StatusCode(429, "Превышен лимит запросов на создание пользователя");

        try
        {
            var result = await userService.CreateUserAsync(dto, dto.Password);
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception($"При создании пользователя произошла ошибка: {ex.Message}");
        }
        finally
        {
            semaphore.Release("create_user");
        }
    }

    /// <summary>
    ///     Удаляет пользователя по его уникальному идентификатору
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя для удаления</param>
    /// <returns>Результат действия, указывающий на успешность или неудачу операции удаления</returns>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType<IdentityResult>(200)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        var semaphoreAcquired = await semaphore.AcquireAsync("delete_user");
        if (!semaphoreAcquired)
            return StatusCode(429, $"Превышен лимит запросов на удаление пользователя с Id {userId}");

        try
        {
            var result = await userService.DeleteUserAsync(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception($"При удалении пользователя произошла ошибка: {ex.Message}");
        }
        finally
        {
            semaphore.Release("delete_user");
        }
    }

    /// <summary>
    ///     Обновляет информацию о пользователе
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя для обновления</param>
    /// <param name="dto">DTO, содержащий обновленную информацию о пользователе</param>
    /// <returns>Результат действия, указывающий на успешность или неудачу операции обновления</returns>
    [HttpPut("{userId:guid}")]
    [ProducesResponseType<IdentityResult>(200)]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var semaphoreAcquired = await semaphore.AcquireAsync("update_user");
        if (!semaphoreAcquired)
            return StatusCode(429, $"Превышен лимит запросов на обновление пользователя с Id {userId}");

        try
        {
            var result = await userService.UpdateUserAsync(userId, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw new Exception($"При обновлении пользователя произошла ошибка: {ex.Message}");
        }
        finally
        {
            semaphore.Release("update_user");
        }
    }

    /// <summary>
    ///     Добавляет пользователя в проект
    /// </summary>
    /// <param name="projectId">Уникальный идентификатор проекта для присоединения</param>
    /// <param name="memberId">Уникальный идентификатор пользователя для добавления в проект</param>
    /// <returns>Результат действия, указывающий на успешность операции</returns>
    [HttpPost("{memberId:guid}/projects/{projectId:guid}")]
    [ProducesResponseType<IdentityResult>(200)]
    public async Task<IActionResult> AddProjectToListOfUserProjects(
        [FromRoute] Guid projectId,
        [FromRoute] Guid memberId)
    {
        var result = await addProjectId.AddProjectIdToProjectIdListAsync(projectId, memberId);
        return Ok(result);
    }
}