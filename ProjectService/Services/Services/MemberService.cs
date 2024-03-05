using AutoMapper;
using Domain.Interfaces;
using Services.Interfaces;
using Services.ViewModels.MemberViewModels;

namespace Services.Services;

/// <summary>
/// Сервис для обработки операций, связанных с участниками проекта
/// </summary>
public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public MemberService(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получает информацию о участнике проекта по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор участника проекта</param>
    /// <returns>Модель представления участника проекта или null, если участник не найден</returns>
    public async Task<MemberViewModel?> GetById(string id)
    {
        var existingMember = await _memberRepository.GetMemberByIdAsync(id);
        return _mapper.Map<MemberViewModel>(existingMember);
    }
}