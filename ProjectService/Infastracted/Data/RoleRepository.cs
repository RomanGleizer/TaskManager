using Domain.Entities;
using Domain.Interfaces;
using Infastracted.EF;
using Microsoft.EntityFrameworkCore;

namespace Infastracted.Data;

public class RoleRepository : IRoleRepository
{
    private readonly ProjectServiceDbContext _context;

    public RoleRepository(ProjectServiceDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetRoleByIdAsync(int roleId)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
    }
}
