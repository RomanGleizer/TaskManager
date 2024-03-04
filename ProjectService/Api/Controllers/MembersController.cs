using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.MemberViewModels;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet("{memberId}")]
    [ProducesResponseType<MemberViewModel>(200)]
    public async Task<IActionResult> GetMemberByIdAsync([FromQuery] string memberId)
    {
        var existingMember = await _memberService.GetById(memberId);
        return Ok(existingMember);
    }
}