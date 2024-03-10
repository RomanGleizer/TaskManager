using Dal.Entities;
using Dal.EF;
using Core.Dal.Base;
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

    /// <inheritdoc/>
    public async Task<IList<TaskDal>> GetAllAsync()
    {
        return await _dbContext.Tasks.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<TaskDal?> GetByIdAsync(int id)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <inheritdoc/>
    public async Task<TaskDal?> CreateAsync(TaskDal item)
    {
        var createdEntity = await _dbContext.Tasks.AddAsync(item);
        await SaveChangesAsync();

        return createdEntity.Entity;
    }

    /// <inheritdoc/>
    public async Task<TaskDal?> DeleteAsync(TaskDal item)
    {
        var removedEntity = _dbContext.Tasks.Remove(item);
        await SaveChangesAsync();

        return removedEntity.Entity;
    }

    /// <inheritdoc/>
    public async Task<TaskDal?> UpdateAsync(TaskDal item)
    {
        var entriedEntity = _dbContext.Entry(item);
        entriedEntity.State = EntityState.Modified;
        await SaveChangesAsync();

        return entriedEntity.Entity;
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
