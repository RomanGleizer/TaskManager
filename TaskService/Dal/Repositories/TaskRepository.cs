using Dal.Interfaces;
using Dal.Entities;
using Dal.EF;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

/// <summary>
/// Репозиторий для работы с сущностями ProjectTask
/// </summary>
public class TaskRepository : IRepository<TaskDal, int>
{
    private readonly TaskManagerDbContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр класса TaskRepository с заданным контекстом базы данных
    /// </summary>
    public TaskRepository(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Возвращает все сущности ProjectTask из базы данных
    /// </summary>
    public async Task<IList<TaskDal>> GetAllAsync()
    {
        return await _dbContext.Tasks.ToListAsync();
    }

    /// <summary>
    /// Асинхронно возвращает сущность ProjectTask с заданным идентификатором из базы данных
    /// </summary>
    public async Task<TaskDal?> GetByIdAsync(int id)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Асинхронно создает новую сущность ProjectTask в базе данных
    /// </summary>
    public async Task CreateAsync(TaskDal item)
    {
        await _dbContext.Tasks.AddAsync(item);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Асинхронно удаляет сущность ProjectTask с заданным идентификатором из базы данных
    /// </summary>
    public async Task DeleteAsync(TaskDal item)
    {
        _dbContext.Tasks.Remove(item);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Обновляет существующую сущность ProjectTask в базе данных
    /// </summary>
    public async Task UpdateAsync(TaskDal item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
}
