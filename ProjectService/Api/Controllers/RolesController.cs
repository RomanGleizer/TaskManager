using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления ролями
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Получает роль по её идентификатору
    /// </summary>
    /// <param name="id">Идентификатор роли</param>
    /// <returns>Данные о роли</returns>
    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetRoleByIdAsync([FromRoute] int id)
    {
        var existingRole = await _roleService.GetById(id);
        return Ok(existingRole);
    }
}
