using AutoMapper;
using Logic.Dto.Role;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(RoleService roleService, IMapper mapper) : ControllerBase
{
    private readonly RoleService _roleService = roleService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetAllDtosAsync();
        return Ok(roles);
    }

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

    [HttpPost]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRole(RoleDto roleDto)
    {
        var createdRole = await _roleService.CreateDtoAsync(roleDto);
        return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
    }

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