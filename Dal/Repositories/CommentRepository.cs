using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public class CommentRepository : IRepository<CommentDal, int>
{
    private readonly TaskManagerDbContext _dbContext;

    public CommentRepository(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<CommentDal> GetAll()
    {
        return _dbContext.Comments;
    }

    public async Task<CommentDal?> GetByIdAsync(int id)
    {
        return await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task CreateAsync(CommentDal item)
    {
        await _dbContext.Comments.AddAsync(item);
    }

    public void Delete(CommentDal item)
    {
        _dbContext.Comments.Remove(item);
    }

    public void Update(CommentDal item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
    }
}
