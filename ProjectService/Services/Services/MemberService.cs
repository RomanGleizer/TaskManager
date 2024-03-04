using AutoMapper;
using Domain.Interfaces;
using Services.Interfaces;
using Services.ViewModels.MemberViewModels;
using Services.ViewModels.RoleViewModels;

namespace Services.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public MemberService(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<MemberViewModel?> GetById(string id)
    {
        var existingMember = await _memberRepository.GetMemberByIdAsync(id);
        return _mapper.Map<MemberViewModel>(existingMember);
    }
}
