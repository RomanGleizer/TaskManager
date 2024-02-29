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
    public IEnumerable<CommentDal> GetAll()
    {
        return _dbContext.Comments;
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
    }

    /// <summary>
    /// Удаляет комментарий
    /// </summary>
    /// <param name="item">Комментарий для удаления</param>
    public void Delete(CommentDal item)
    {
        _dbContext.Comments.Remove(item);
    }

    /// <summary>
    /// Обновляет комментарий
    /// </summary>
    /// <param name="item">Комментарий для обновления</param>
    public void Update(CommentDal item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
    }
}
