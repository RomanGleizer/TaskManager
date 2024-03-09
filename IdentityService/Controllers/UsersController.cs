using AutoMapper;
using Logic.Dto.User;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMapper mapper, IUserService<UserDto, Guid> userService) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserService<UserDto, Guid> _userService = userService;

    [HttpGet("{userId}")]
    [ProducesResponseType<UniversalUserResponse>(200)]
    public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid userId)
    {
        var existingUser = await _userService.GetUserByIdAsync(userId);
        return Ok(_mapper.Map<UniversalUserResponse>(existingUser));
    }

    [HttpPost]
    [ProducesResponseType<UniversalUserResponse>(200)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDTO request)
    {
        var userDTO = _mapper.Map<UserDto>(request);
        var createdUser = await _userService.CreateUserAsync(userDTO);
        return Ok(_mapper.Map<UniversalUserResponse>(createdUser));
    }
}
