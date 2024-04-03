using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF;

/// <summary>
/// Представляет контекст базы данных для хранения сущностей
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса TaskManagerDbContext с указанными параметрами
/// </remarks>
/// <param name="options">Опции для этого контекста</param>
public class TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Задачи в базе данных
    /// </summary>
    public DbSet<TaskDal> Tasks { get; set; }
}