using Domain.Entities;
using Domain.Interfaces;
using Infastracted.EF;
using Microsoft.EntityFrameworkCore;

namespace Infastracted.Data;

/// <summary>
/// Реализация интерфейса репозитория для работы с данными ролей
/// </summary>
public class RoleRepository : IRoleRepository
{
    private readonly ProjectServiceDbContext _context;

    public RoleRepository(ProjectServiceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получает роль по её идентификатору
    /// </summary>
    /// <param name="roleId">Идентификатор роли</param>
    /// <returns>Объект роли или null, если роль не найдена</returns>
    public async Task<Role?> GetRoleByIdAsync(int roleId)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
    }
}
