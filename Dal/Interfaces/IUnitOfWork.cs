using Dal.Entities;

namespace Dal.Interfaces;

/// <summary>
/// Интерфейс для реализации Unit of Work, который объединяет несколько операций с базой данных в одной транзакции.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Получает репозиторий для работы с сущностями ProjectTask.
    /// </summary>
    IRepository<TaskDal, int> Tasks { get; }

    /// <summary>
    /// Асинхронно сохраняет все изменения, сделанные в базе данных.
    /// </summary>
    Task SaveChangesAsync();
}
