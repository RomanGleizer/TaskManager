using Dal.Ef;
using Dal.Entities;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    public async Task CreateAsync(RoleDal entity)
    {
        await _context.Roles.AddAsync(entity);
        await SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(RoleDal item)
    {
        _context.Roles.Remove(item);
        await SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(RoleDal entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}