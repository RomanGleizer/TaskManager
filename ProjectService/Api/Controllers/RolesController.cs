using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetRoleByIdAsync([FromQuery] int id)
    {
        var exisitingRole = await _roleService.GetById(id);
        return Ok(exisitingRole);
    }
}
