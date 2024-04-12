using Core.Dal.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsersMicroservice.UsersMicroserviceDal.Entities;
using UsersMicroservice.UsersMicroserviceDal.EntityFramework;

namespace UsersMicroservice.UsersMicroserviceDal.Repositories;

public class UserRepository(UsersMicroserviceDbContext context, UserManager<UserDal> userManager)
    : IUserRepository<UserDal>
{
    /// <inheritdoc />
    public async Task<IList<UserDal>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<UserDal?> GetByIdAsync(Guid id)
    {
        return await userManager.FindByIdAsync(id.ToString());
    }

    /// <inheritdoc />
    public async Task<IdentityResult?> CreateAsync(UserDal entity, string password)
    {
        var createdEntity = await userManager.CreateAsync(entity, password);
        return createdEntity;
    }

    /// <inheritdoc />
    public async Task<IdentityResult?> DeleteAsync(UserDal entity)
    {
        var deletedEntity = await userManager.DeleteAsync(entity);
        return deletedEntity;
    }

    /// <inheritdoc />
    public async Task<IdentityResult?> UpdateAsync(UserDal entity)
    {
        var updatedEntity = await userManager.UpdateAsync(entity);
        return updatedEntity;
    }
}