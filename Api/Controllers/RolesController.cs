using Api.Controllers.Role.Requests;
using Api.Controllers.Role.Responses;
using Logic.DTO;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления ролями пользователей
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IDtoService<RoleDTO, int> _roleService;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера RolesController
    /// </summary>
    /// <param name="roleService">Сервис ролей пользователей</param>
    public RolesController(IDtoService<RoleDTO, int> roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Получает информацию о роли по ее идентификатору
    /// </summary>
    /// <param name="roleId">Идентификатор роли</param>
    [HttpGet("{roleId}")]
    [ProducesResponseType<RoleInfoResponse>(200)]
    public async Task<IActionResult> GetInfoAsync([FromRoute] int roleId)
    {
        var role = await _roleService.GetDtoByIdAsync(roleId);
        return Ok(new RoleInfoResponse
        {
            Id = role.Id,
            Name = role.Name,
            UserIds = role.UserIds
        });
    }

    /// <summary>
    /// Создает новую роль
    /// </summary>
    /// <param name="request">Запрос на создание роли</param>
    [HttpPost]
    [ProducesResponseType<CreateRoleResponse>(200)]
    public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleRequest request)
    {
        var newRole = await _roleService.CreateDtoAsync(new RoleDTO
        {
            Id = request.Id,
            Name = request.Name,
            UserIds = request.UserIds
        });
        return Ok(new CreateRoleResponse
        {
            Id = newRole.Id,
            Name = newRole.Name,
            UserIds = newRole.UserIds
        });
    }

    /// <summary>
    /// Удаляет роль по ее идентификатору
    /// </summary>
    /// <param name="roleId">Идентификатор роли</param>
    [HttpDelete("{roleId}")]
    [ProducesResponseType<DeleteRoleResponse>(200)]
    public async Task<IActionResult> DeleteRoleAsync([FromRoute] int roleId)
    {
        var deletedRole = await _roleService.DeleteDtoAsync(roleId);
        return Ok(new DeleteRoleResponse
        {
            Id = deletedRole.Id,
            Name = deletedRole.Name,
            UserIds = deletedRole.UserIds
        });
    }

    /// <summary>
    /// Обновляет информацию о роли
    /// </summary>
    /// <param name="request">Запрос на обновление информации о роли</param>
    /// <param name="roleId">Идентификатор роли</param>
    [HttpPut("{roleId}")]
    [ProducesResponseType<UpdateRoleResponse>(200)]
    public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleRequest request, [FromRoute] int roleId)
    {
        var roleDTO = new RoleDTO
        {
            Id = request.Id,
            Name = request.Name,
            UserIds = request.UserIds
        };

        var updatedUserDal = await _roleService.UpdateDtoAsync(roleDTO, roleId);
        return Ok(new UpdateRoleResponse
        {
            Id = request.Id,
            Name = request.Name,
            UserIds = request.UserIds
        });
    }
}
