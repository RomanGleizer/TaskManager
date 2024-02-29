using Api.Controllers.User.Requests;
using Api.Controllers.User.Responses;
using Logic.DTO;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления пользователями
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IDtoService<UserDTO, string> _userService;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера UsersController
    /// </summary>
    /// <param name="userService">Сервис пользователей</param>
    public UsersController(IDtoService<UserDTO, string> userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Получает информацию о пользователе по его идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    [HttpGet("{userId}")]
    [ProducesResponseType<UserInfoResponse>(200)]
    public async Task<IActionResult> GetInfoAsync([FromRoute] string userId)
    {
        var user = await _userService.GetDtoByIdAsync(userId);
        return Ok(new UserInfoResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            BirthDay = user.BirthDay,
            RoleId = user.RoleId,
        });
    }

    /// <summary>
    /// Создает нового пользователя
    /// </summary>
    /// <param name="request">Запрос на создание пользователя</param>
    [HttpPost]
    [ProducesResponseType<CreateUserResponse>(200)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var newUser = await _userService.CreateDtoAsync(new UserDTO
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            BirthDay = request.BirthDay,
            RoleId = request.RoleId
        });
        return Ok(new CreateUserResponse
        {
            Id = newUser.Id,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            PhoneNumber = newUser.PhoneNumber,
            BirthDay = newUser.BirthDay,
            RoleId = newUser.RoleId
        });
    }

    /// <summary>
    /// Удаляет пользователя по его идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    [HttpDelete("{userId}")]
    [ProducesResponseType<DeleteUserResponse>(200)]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] string userId)
    {
        var deletedUser = await _userService.DeleteDtoAsync(userId);
        return Ok(new DeleteUserResponse
        {
            Id = deletedUser.Id,
            FirstName = deletedUser.FirstName,
            LastName = deletedUser.LastName,
            Email = deletedUser.Email,
            PhoneNumber = deletedUser.PhoneNumber,
            BirthDay = deletedUser.BirthDay,
            RoleId = deletedUser.RoleId
        });
    }

    /// <summary>
    /// Обновляет информацию о пользователе
    /// </summary>
    /// <param name="request">Запрос на обновление информации о пользователе</param>
    /// <param name="userId">Идентификатор пользователя</param>
    [HttpPut("{userId}")]
    [ProducesResponseType<UpdateUserResponse>(200)]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserRequest request, [FromRoute] string userId)
    {
        var userDTO = new UserDTO
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            BirthDay = request.BirthDay,
            RoleId = request.RoleId
        };

        var updatedUserDal = await _userService.UpdateDtoAsync(userDTO, userId);
        return Ok(new UpdateUserResponse
        {
            Id = updatedUserDal.Id,
            FirstName = updatedUserDal.FirstName,
            LastName = updatedUserDal.LastName,
            Email = updatedUserDal.Email,
            PhoneNumber = updatedUserDal.PhoneNumber,
            BirthDay = updatedUserDal.BirthDay,
            RoleId = updatedUserDal.RoleId
        });
    }
}
