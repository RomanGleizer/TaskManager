using Core.Dal.Base;
using Logic.Dto.User;
using Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления операциями, связанными с пользователями
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService, IAddProjectIdToUserProjectIdList addProjectId) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IAddProjectIdToUserProjectIdList _addProjectIdToProjectIdList = addProjectId;

    /// <summary>
    /// Получает всех пользователей из базы данных
    /// </summary>
    /// <returns>Результат действия, содержащий коллекцию пользователей</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<UserDto>>(200)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Получает пользователя по его уникальному идентификатору
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя для получения</param>
    /// <returns>Результат действия, содержащий информацию о пользователе, если найден, в противном случае возвращает NotFound</returns>
    [HttpGet("{userId}")]
    [ProducesResponseType<UserDto>(200)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
    {
        var result = await _userService.GetUserByIdAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Создает нового пользователя
    /// </summary>
    /// <param name="dto">DTO, представляющий пользователя, который должен быть создан</param>
    /// <returns>Результат действия, указывающий на успешность или неудачу операции создания</returns>
    [HttpPost]
    [ProducesResponseType<IdentityResult>(200)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _userService.CreateUserAsync(dto, dto.Password);
        return Ok(result);
    }

    /// <summary>
    /// Удаляет пользователя по его уникальному идентификатору
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя для удаления</param>
    /// <returns>Результат действия, указывающий на успешность или неудачу операции удаления</returns>
    [HttpDelete("{userId}")]
    [ProducesResponseType<IdentityResult>(200)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        var result = await _userService.DeleteUserAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Обновляет информацию о пользователе
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя для обновления</param>
    /// <param name="dto">DTO, содержащий обновленную информацию о пользователе</param>
    /// <returns>Результат действия, указывающий на успешность или неудачу операции обновления</returns>
    [HttpPut("{userId}")]
    [ProducesResponseType<IdentityResult>(200)]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _userService.UpdateUserAsync(userId, dto);

        return Ok(result);
    }

    /// <summary>
    /// Добавляет пользователя в проект
    /// </summary>
    /// <param name="projectId">Уникальный идентификатор проекта для присоединения</param>
    /// <param name="memberId">Уникальный идентификатор пользователя для добавления в проект</param>
    /// <returns>Результат действия, указывающий на успешность операции</returns>
    [HttpPost("{memberId}/projects/{projectId}")]
    [ProducesResponseType<IdentityResult>(200)]
    public async Task<IActionResult> AddProjectToListOfUserProjects([FromRoute] Guid projectId, [FromRoute] Guid memberId)
    {
        var result = await _addProjectIdToProjectIdList.AddProjectIdToProjectIdListAsync(projectId, memberId);
        return Ok(result);
    }
}
