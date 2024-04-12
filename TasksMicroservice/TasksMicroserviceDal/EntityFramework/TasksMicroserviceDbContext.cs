using Microsoft.EntityFrameworkCore;
using TasksMicroservice.TasksMicroserviceDal.Entities;

namespace TasksMicroservice.TasksMicroserviceDal.EntityFramework;

/// <summary>
///     Представляет контекст базы данных для хранения сущностей
/// </summary>
/// <remarks>
///     Инициализирует новый экземпляр класса TaskManagerDbContext с указанными параметрами
/// </remarks>
/// <param name="options">Опции для этого контекста</param>
public class TasksMicroserviceDbContext : DbContext
{
    public TasksMicroserviceDbContext(DbContextOptions<TasksMicroserviceDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    ///     Задачи в базе данных
    /// </summary>
    public DbSet<TaskDal> Tasks { get; set; }
}