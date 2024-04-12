using Microsoft.AspNetCore.Mvc;
using UsersMicroservice.UsersMicroserviceLogic.Dto.Role;
using UsersMicroservice.UsersMicroserviceLogic.Services;

namespace UsersMicroservice.UsersMicroserviceApi.Controllers;

/// <summary>
///     Контроллер для управления ролями.
/// </summary>
/// <remarks>
///     Инициализирует новый экземпляр контроллера ролей
/// </remarks>
/// <param name="roleService">Сервис ролей</param>
[Route("api/[controller]")]
[ApiController]
public class RolesController(RoleService roleService) : ControllerBase
{
    /// <summary>
    ///     Получает все роли
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await roleService.GetAllDtosAsync();
        return Ok(roles);
    }

    /// <summary>
    ///     Получает роль по идентификатору
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoleById(int id)
    {
        var role = await roleService.GetDtoByIdAsync(id);
        return Ok(role);
    }

    /// <summary>
    ///     Создает новую роль
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRole(RoleDto roleDto)
    {
        var createdRole = await roleService.CreateDtoAsync(roleDto);
        return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
    }

    /// <summary>
    ///     Обновляет роль
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRole(int id, RoleDto roleDto)
    {
        var updatedRole = await roleService.UpdateDtoAsync(roleDto, id);
        return Ok(updatedRole);
    }

    /// <summary>
    ///     Удаляет роль
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var deletedRole = await roleService.DeleteDtoAsync(id);
        return Ok(deletedRole);
    }
}