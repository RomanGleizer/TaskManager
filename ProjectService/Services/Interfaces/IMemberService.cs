using Services.ViewModels.MemberViewModels;

namespace Services.Interfaces;

public interface IMemberService
{
    Task<MemberViewModel?> GetById(string id);
}
