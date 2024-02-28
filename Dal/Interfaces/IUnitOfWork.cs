using Dal.Entities;

namespace Dal.Interfaces;

/// <summary>
/// Интерфейс для реализации Unit of Work, который объединяет несколько операций с базой данных в одной транзакции
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Репозиторий для работы с задачами
    /// </summary>
    IRepository<TaskDal, int> Tasks { get; }

    /// <summary>
    /// Репозиторий для работы с сущностями проектами
    /// </summary>
    IRepository<ProjectDal, int> Projects { get; }

    /// <summary>
    /// Репозиторий для работы с комментариями
    /// </summary>
    IRepository<CommentDal, int> Comments { get; }

    /// <summary>
    /// Репозиторий для работы с пользователями
    /// </summary>
    IRepository<UserDal, string> Users { get; }

    /// <summary>
    /// Асинхронно сохраняет все изменения, сделанные в базе данных
    /// </summary>
    Task SaveChangesAsync();
}
