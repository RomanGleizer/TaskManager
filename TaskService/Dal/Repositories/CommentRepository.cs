using Core.Dal.Base;
using Dal.EF;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

/// <summary>
/// Репозиторий для работы с комментариями
/// </summary>
/// <remarks>
/// Конструктор репозитория комментариев
/// </remarks>
/// <param name="dbContext">Контекст базы данных</param>
public class CommentRepository(TaskManagerDbContext dbContext) : IRepository<CommentDal, int>
{
    private readonly TaskManagerDbContext _dbContext = dbContext;

    /// <inheritdoc/>
    public async Task<IList<CommentDal>> GetAllAsync()
    {
        return await _dbContext.Comments.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<CommentDal?> GetByIdAsync(int id)
    {
        return await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <inheritdoc/>
    public async Task<CommentDal?> CreateAsync(CommentDal item)
    {
        var createdComment = await _dbContext.Comments.AddAsync(item);
        await SaveChangesAsync();

        return createdComment.Entity;
    }

    /// <inheritdoc/>
    public async Task<CommentDal?> UpdateAsync(CommentDal item)
    {
        var entriedEntry = _dbContext.Entry(item);
        entriedEntry.State = EntityState.Modified;
        await SaveChangesAsync();

        return entriedEntry.Entity;
    }

    /// <inheritdoc/>
    public async Task<CommentDal?> DeleteAsync(CommentDal item)
    {
        var removedEntity = _dbContext.Comments.Remove(item);
        await SaveChangesAsync();

        return removedEntity.Entity;
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
