using Domain.Entities;

namespace Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetRoleByIdAsync(int roleId);
}
