using Core.Dal.Base;
using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;

namespace Dal.Repositories;

/// <summary>
/// Реализация интерфейса IUnitOfWork для работы с базой данных с использованием EF
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса EFUnitOfWork с заданным контекстом базы данных
/// </remarks>
public class EFUnitOfWork(TaskManagerDbContext dbContext) : IUnitOfWork
{
    private readonly TaskManagerDbContext _dbContext = dbContext;
    private IRepository<TaskDal, int>? _taskRepository;
    private IRepository<CommentDal, int>? _commentRepository;

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
            _commentRepository ??= new CommentRepository(_dbContext);
            return _commentRepository;
        }
    }
}
