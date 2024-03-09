using Dal.Ef;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Dal.Base;

namespace Dal.Repositories;

/// <summary>
/// Представляет репозиторий для работы с ролями
/// </summary>
public class RoleRepository : IRepository<RoleDal, int>
{
    private readonly IdentityServiceDbContext _context;

    /// <summary>
    /// Инициализирует новый экземпляр класса RoleRepository с указанным контекстом базы данных
    /// </summary>
    /// <param name="context">Контекст базы данных Entity Framework</param>
    public RoleRepository(IdentityServiceDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<IList<RoleDal>> GetAllAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<RoleDal?> GetByIdAsync(int id)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <inheritdoc/>
    public async Task<RoleDal?> CreateAsync(RoleDal entity)
    {
        var createdRole = await _context.Roles.AddAsync(entity);
        await SaveChangesAsync();

        return createdRole.Entity;
    }

    /// <inheritdoc/>
    public async Task<RoleDal?> DeleteAsync(RoleDal item)
    {
        var deletedRole = _context.Roles.Remove(item);
        await SaveChangesAsync();

        return deletedRole.Entity;
    }

    /// <inheritdoc/>
    public async Task<RoleDal?> UpdateAsync(RoleDal entity)
    {
        var entriedRole = _context.Entry(entity);
        entriedRole.State = EntityState.Modified;
        await SaveChangesAsync();

        return entriedRole.Entity;
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}