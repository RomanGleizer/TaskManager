using Core.Dal.Base;
using Microsoft.EntityFrameworkCore;
using UsersMicroservice.UsersMicroserviceDal.Entities;
using UsersMicroservice.UsersMicroserviceDal.EntityFramework;

namespace UsersMicroservice.UsersMicroserviceDal.Repositories;

/// <summary>
///     Представляет репозиторий для работы с ролями
/// </summary>
/// <remarks>
///     Инициализирует новый экземпляр класса RoleRepository с указанным контекстом базы данных
/// </remarks>
/// <param name="context">Контекст базы данных Entity Framework</param>
public class RoleRepository(UsersMicroserviceDbContext context) : IRepository<RoleDal, int>
{
    /// <inheritdoc />
    public async Task<IList<RoleDal>> GetAllAsync()
    {
        return await context.Roles.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<RoleDal?> GetByIdAsync(int id)
    {
        return await context.Roles.FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <inheritdoc />
    public async Task<RoleDal?> CreateAsync(RoleDal entity)
    {
        var createdRole = await context.Roles.AddAsync(entity);
        await SaveChangesAsync();

        return createdRole.Entity;
    }

    /// <inheritdoc />
    public async Task<RoleDal?> DeleteAsync(RoleDal item)
    {
        var deletedRole = context.Roles.Remove(item);
        await SaveChangesAsync();

        return deletedRole.Entity;
    }

    /// <inheritdoc />
    public async Task<RoleDal?> UpdateAsync(RoleDal entity)
    {
        var entriedRole = context.Entry(entity);
        entriedRole.State = EntityState.Modified;
        await SaveChangesAsync();

        return entriedRole.Entity;
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}