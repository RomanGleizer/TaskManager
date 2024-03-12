using AutoMapper;
using ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;
using Logic.Dto.User;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления операциями, связанными с пользователями
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController(IMapper mapper, IUserService userService) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserService _userService = userService;

    /// <summary>
    /// Получает всех пользователей из базы данных
    /// </summary>
    /// <returns>Результат действия, содержащий коллекцию пользователей</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
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
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    /// <summary>
    /// Создает нового пользователя
    /// </summary>
    /// <param name="dto">DTO, представляющий пользователя, который должен быть создан</param>
    /// <returns>Результат действия, указывающий на успешность или неудачу операции создания</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.CreateUserAsync(dto, dto.Password);
        if (result != null && result.Succeeded)
            return CreatedAtAction(nameof(GetUserById), result);

        return BadRequest();
    }

    /// <summary>
    /// Удаляет пользователя по его уникальному идентификатору
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя для удаления</param>
    /// <returns>Результат действия, указывающий на успешность или неудачу операции удаления</returns>
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        var result = await _userService.DeleteUserAsync(userId);
        if (result != null && result.Succeeded)
            return Ok();

        return NotFound();
    }

    /// <summary>
    /// Обновляет информацию о пользователе
    /// </summary>
    /// <param name="userId">Уникальный идентификатор пользователя для обновления</param>
    /// <param name="dto">DTO, содержащий обновленную информацию о пользователе</param>
    /// <returns>Результат действия, указывающий на успешность или неудачу операции обновления</returns>
    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.UpdateUserAsync(userId, dto);
        if (result != null && result.Succeeded)
            return Ok();

        return NotFound();
    }

    /// <summary>
    /// Добавляет пользователя в проект
    /// </summary>
    /// <param name="projectId">Уникальный идентификатор проекта для присоединения</param>
    /// <param name="memberId">Уникальный идентификатор пользователя для добавления в проект</param>
    /// <returns>Результат действия, указывающий на успешность операции</returns>
    [HttpPost("{memberId}/projects/{projectId}")]
    [ProducesResponseType(typeof(AddTaskInProjectApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> JoinInProject([FromRoute] int projectId, [FromRoute] Guid memberId)
    {
        await _userService.AddNewProject(projectId, memberId);
        return Ok();
    }
}
