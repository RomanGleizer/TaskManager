using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public class UserRepository : IRepository<UserDal, string>
{
    private readonly TaskManagerDbContext _dbContext;

    public UserRepository(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<UserDal> GetAll()
    {
        return _dbContext.Users;
    }

    public async Task<UserDal?> GetByIdAsync(string id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }  

    public async Task CreateAsync(UserDal item)
    {
        await _dbContext.Users.AddAsync(item);
    }

    public void Delete(UserDal item)
    {
        _dbContext.Users.Remove(item);
    }

    public void Update(UserDal item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
    }
}
