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
    public async Task<UserDal?> CreateAsync(UserDal entity)
    {
        var createdEntity = await _context.Users.AddAsync(entity);
        await SaveChangesAsync();
        return createdEntity.Entity;
    }

    /// <inheritdoc/>
    public async Task<UserDal?> DeleteAsync(UserDal item)
    {
        var removedEntity = _context.Users.Remove(item);
        await SaveChangesAsync();
        return removedEntity.Entity;
    }

    /// <inheritdoc/>
    public async Task<UserDal?> UpdateAsync(UserDal entity)
    {
        var entriedUser = _context.Entry(entity);
        entriedUser.State = EntityState.Modified;
        await SaveChangesAsync();

        return entriedUser.Entity;
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}