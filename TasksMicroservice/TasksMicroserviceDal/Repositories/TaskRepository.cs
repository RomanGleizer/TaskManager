using Core.Dal.Base;
using Microsoft.EntityFrameworkCore;
using TasksMicroservice.TasksMicroserviceDal.Entities;
using TasksMicroservice.TasksMicroserviceDal.EntityFramework;

namespace TasksMicroservice.TasksMicroserviceDal.Repositories;

/// <summary>
///     Репозиторий для работы с сущностями ProjectTask
/// </summary>
/// <remarks>
///     Инициализирует новый экземпляр класса TaskRepository с заданным контекстом базы данных
/// </remarks>
public class TaskRepository(TasksMicroserviceDbContext dbContext) : IRepository<TaskDal, Guid>
{
    /// <inheritdoc />
    public async Task<IList<TaskDal>> GetAllAsync()
    {
        return await dbContext.Tasks.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<TaskDal?> GetByIdAsync(Guid id)
    {
        return await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <inheritdoc />
    public async Task<TaskDal?> CreateAsync(TaskDal item)
    {
        var createdEntity = await dbContext.Tasks.AddAsync(item);
        await SaveChangesAsync();

        return createdEntity.Entity;
    }

    /// <inheritdoc />
    public async Task<TaskDal?> DeleteAsync(TaskDal item)
    {
        var removedEntity = dbContext.Tasks.Remove(item);
        await SaveChangesAsync();

        return removedEntity.Entity;
    }

    /// <inheritdoc />
    public async Task<TaskDal?> UpdateAsync(TaskDal item)
    {
        var entityEntry = dbContext.Entry(item);
        entityEntry.State = EntityState.Modified;
        await SaveChangesAsync();

        return entityEntry.Entity;
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}