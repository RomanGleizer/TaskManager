using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF;

/// <summary>
/// Представляет контекст базы данных для хранения сущностей
/// </summary>
public class TaskManagerDbContext : DbContext
{
    /// <summary>
    /// Задачи в базе данных
    /// </summary>
    public DbSet<TaskDal> Tasks { get; set; }

    /// <summary>
    /// Комментарии к задачам
    /// </summary>
    public DbSet<CommentDal> Comments { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса TaskManagerDbContext с указанными параметрами
    /// </summary>
    /// <param name="options">Опции для этого контекста</param>
    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options) { }
}