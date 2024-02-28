using Dal.Interfaces;
using Dal.Entities;
using Dal.EF;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

/// <summary>
/// Репозиторий для работы с сущностями ProjectTask.
/// </summary>
public class TaskRepository : IRepository<TaskDal, int>
{
    private readonly TaskManagerDbContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр класса TaskRepository с заданным контекстом базы данных.
    /// </summary>
    public TaskRepository(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Возвращает все сущности ProjectTask из базы данных.
    /// </summary>
    public IEnumerable<TaskDal> GetAll()
    {
        return _dbContext.Tasks;
    }

    /// <summary>
    /// Асинхронно возвращает сущность ProjectTask с заданным идентификатором из базы данных.
    /// </summary>
    public async Task<TaskDal?> GetByIdAsync(int id)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Асинхронно создает новую сущность ProjectTask в базе данных.
    /// </summary>
    public async Task CreateAsync(TaskDal item)
    {
        await _dbContext.Tasks.AddAsync(item);
    }

    /// <summary>
    /// Асинхронно удаляет сущность ProjectTask с заданным идентификатором из базы данных.
    /// </summary>
    public void Delete(TaskDal item)
    {
        _dbContext.Tasks.Remove(item);
    }

    /// <summary>
    /// Обновляет существующую сущность ProjectTask в базе данных.
    /// </summary>
    public void Update(TaskDal item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
    }
}
