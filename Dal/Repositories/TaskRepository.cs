using Dal.Interfaces;
using Dal.Entities;
using Dal.EF;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

/// <summary>
/// Репозиторий для работы с сущностями ProjectTask.
/// </summary>
public class TaskRepository : IRepository<ProjectTask, int>
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
    /// Асинхронно создает новую сущность ProjectTask в базе данных.
    /// </summary>
    public async Task CreateTaskAsync(ProjectTask item)
    {
        await _dbContext.Tasks.AddAsync(item);
    }

    /// <summary>
    /// Асинхронно удаляет сущность ProjectTask с заданным идентификатором из базы данных.
    /// </summary>
    public async Task DeleteTaskAsync(int id)
    {
        var task = await _dbContext.Tasks.FindAsync(id);
        if (task != null)
            _dbContext.Tasks.Remove(task);
    }

    /// <summary>
    /// Возвращает все сущности ProjectTask из базы данных.
    /// </summary>
    public IEnumerable<ProjectTask> GetAllTasks()
    {
        return _dbContext.Tasks;
    }

    /// <summary>
    /// Асинхронно возвращает сущность ProjectTask с заданным идентификатором из базы данных.
    /// </summary>
    public async Task<ProjectTask?> GetTaskById(int id)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Обновляет существующую сущность ProjectTask в базе данных.
    /// </summary>
    public void UpdateTaskAsync(ProjectTask item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
    }
}
