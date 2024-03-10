using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;

namespace Dal.Repositories;

/// <summary>
/// Реализация интерфейса IUnitOfWork для работы с базой данных с использованием EF
/// </summary>
public class EFUnitOfWork : IUnitOfWork
{
    private readonly TaskManagerDbContext _dbContext;
    private IRepository<TaskDal, int>? _taskRepository;
    private IRepository<CommentDal, int>? _commentRepository;

    /// <summary>
    /// Инициализирует новый экземпляр класса EFUnitOfWork с заданным контекстом базы данных
    /// </summary>
    public EFUnitOfWork(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получает репозиторий для работы с сущностями TaskDal
    /// </summary>
    public IRepository<TaskDal, int> Tasks
    {
        get
        {
            if (_taskRepository == null)
                _taskRepository = new TaskRepository(_dbContext);
            return _taskRepository;
        }
    }

    /// <summary>
    /// Получает репозиторий для работы с сущностями CommentDal
    /// </summary>
    public IRepository<CommentDal, int> Comments
    {
        get
        {
            if (_commentRepository == null)
                _commentRepository = new CommentRepository(_dbContext);
            return _commentRepository;
        }
    }
}
