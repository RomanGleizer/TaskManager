using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

/// <summary>
/// Репозиторий для работы с комментариями
/// </summary>
public class CommentRepository : IRepository<CommentDal, int>
{
    private readonly TaskManagerDbContext _dbContext;

    /// <summary>
    /// Конструктор репозитория комментариев
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    public CommentRepository(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получает все комментарии
    /// </summary>
    public async Task<IList<CommentDal>> GetAllAsync()
    {
        return await _dbContext.Comments.ToListAsync();
    }

    /// <summary>
    /// Получает комментарий по идентификатору асинхронно
    /// </summary>
    /// <param name="id">Идентификатор комментария</param>
    public async Task<CommentDal?> GetByIdAsync(int id)
    {
        return await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Создает комментарий асинхронно
    /// </summary>
    /// <param name="item">Комментарий для создания</param>
    public async Task CreateAsync(CommentDal item)
    {
        await _dbContext.Comments.AddAsync(item);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Асинхронно обновляет комментарий
    /// </summary>
    /// <param name="item">Комментарий для обновления</param>
    public async Task UpdateAsync(CommentDal item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Асинхронно Удаляет комментарий
    /// </summary>
    /// <param name="item">Комментарий для удаления</param>
    public async Task DeleteAsync(CommentDal item)
    {
        _dbContext.Comments.Remove(item);
        await _dbContext.SaveChangesAsync();
    }
}
