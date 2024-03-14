using AutoMapper;
using Logic.Dto.Role;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления ролями.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly RoleService _roleService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера ролей
    /// </summary>
    /// <param name="roleService">Сервис ролей</param>
    /// <param name="mapper">Маппер</param>
    public RolesController(RoleService roleService, IMapper mapper)
    {
        _roleService = roleService;
        _mapper = mapper;
    }

    /// <summary>
    /// Получает все роли
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetAllDtosAsync();
        return Ok(roles);
    }

    /// <summary>
    /// Получает роль по идентификатору
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleById(int id)
    {
        var role = await _roleService.GetDtoByIdAsync(id);
        if (role == null)
            return NotFound("Role not found");

        return Ok(role);
    }

    /// <summary>
    /// Создает новую роль
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRole(RoleDto roleDto)
    {
        var createdRole = await _roleService.CreateDtoAsync(roleDto);
        return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
    }

    /// <summary>
    /// Обновляет роль
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRole(int id, RoleDto roleDto)
    {
        var updatedRole = await _roleService.UpdateDtoAsync(roleDto, id);
        if (updatedRole == null)
            return NotFound("Role not found");

        return Ok(updatedRole);
    }

    /// <summary>
    /// Удаляет роль
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var deletedRole = await _roleService.DeleteDtoAsync(id);
        if (deletedRole == null)
            return NotFound("Role not found");

        return Ok(deletedRole);
    }
}