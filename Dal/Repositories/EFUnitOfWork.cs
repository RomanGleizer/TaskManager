using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;

namespace Dal.Repositories;

/// <summary>
/// Реализация интерфейса IUnitOfWork для работы с базой данных с использованием EF.
/// </summary>
public class EFUnitOfWork : IUnitOfWork
{
    private readonly TaskManagerDbContext _dbContext;
    private IRepository<ProjectTask, int>? _taskRepository;

    /// <summary>
    /// Инициализирует новый экземпляр класса EFUnitOfWork с заданным контекстом базы данных.
    /// </summary>
    public EFUnitOfWork(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получает репозиторий для работы с сущностями ProjectTask.
    /// </summary>
    public IRepository<ProjectTask, int> Tasks
    {
        get
        {
            if (_taskRepository == null)
                _taskRepository = new TaskRepository(_dbContext);
            return _taskRepository;
        }
    }

    /// <summary>
    /// Асинхронно сохраняет все изменения, сделанные в базе данных.
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
