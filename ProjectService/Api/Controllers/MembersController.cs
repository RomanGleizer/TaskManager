using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels.MemberViewModels;

namespace Api.Controllers;

/// <summary>
/// Контроллер для обработки операций, связанных с участниками проекта
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    /// <summary>
    /// Получает участника по его идентификатору
    /// </summary>
    /// <param name="memberId">Идентификатор участника для получения</param>
    /// <returns>Данные об участнике</returns>
    [HttpGet("{memberId}")]
    [ProducesResponseType<MemberViewModel>(200)]
    public async Task<IActionResult> GetMemberByIdAsync([FromRoute] string memberId)
    {
        var existingMember = await _memberService.GetById(memberId);
        return Ok(existingMember);
    }
}