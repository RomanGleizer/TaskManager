using Core.Dal.Base;
using Dal.Ef;
using Dal.Entities;
using Dal.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public class UserRepository(IdentityServiceDbContext context, UserManager<UserDal> userManager) : IUserRepository<UserDal>
{
    private readonly IdentityServiceDbContext _context = context;
    private readonly UserManager<UserDal> _userManager = userManager;

    /// <inheritdoc/>
    public async Task<IList<UserDal>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<UserDal?> GetByIdAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString());
    }

    /// <inheritdoc/>
    public async Task<IdentityResult?> CreateAsync(UserDal entity, string password)
    {
        var createdEntity = await _userManager.CreateAsync(entity, password);
        return createdEntity;
    }

    /// <inheritdoc/>
    public async Task<IdentityResult?> DeleteAsync(UserDal entity)
    {
        var deletedEntity = await _userManager.DeleteAsync(entity);
        return deletedEntity;
    }

    /// <inheritdoc/>
    public async Task<IdentityResult?> UpdateAsync(UserDal entity)
    {
        var updatedEntity = await _userManager.UpdateAsync(entity);
        return updatedEntity;
    }
}
