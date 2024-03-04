using Services.ViewModels.MemberViewModels;

namespace Services.Interfaces;

public interface IRoleService
{
    Task<MemberViewModel?> GetById(int id);
}
