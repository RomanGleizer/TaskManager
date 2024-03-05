using Services.ViewModels.RoleViewModels;

namespace Services.Interfaces;

public interface IMemberService
{
    Task<RoleViewModel?> GetById(string id);
}
