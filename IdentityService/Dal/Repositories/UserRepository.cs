using Dal.Ef;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Dal.Base;

namespace Dal.Repositories;

/// <summary>
/// Представляет репозиторий для работы с пользователями
/// </summary>
public class UserRepository : IRepository<UserDal, Guid>
{
    private readonly IdentityServiceDbContext _context;

    /// <summary>
    /// Инициализирует новый экземпляр класса UserRepository с указанным контекстом базы данных
    /// </summary>
    /// <param name="context">Контекст базы данных Entity Framework</param>
    public UserRepository(IdentityServiceDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<IList<UserDal>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<UserDal?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <inheritdoc/>
    public async Task CreateAsync(UserDal entity)
    {
        await _context.Users.AddAsync(entity);
        await SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(UserDal item)
    {
        _context.Users.Remove(item);
        await SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(UserDal entity)
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