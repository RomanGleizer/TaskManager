using Dal.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF;

/// <summary>
/// Описание:
/// Представляет контекст базы данных для хранения сущностей.
/// </summary>
public class TaskManagerDbContext : IdentityDbContext<User>
{
    /// <summary>
    /// Пользователи в базе данных.
    /// </summary>
    public override DbSet<User> Users { get; set; }

    /// <summary>
    /// Задачи в базе данных
    /// </summary>
    public DbSet<ProjectTask> Tasks { get; set; }

    /// <summary>
    /// Проекты в базе данных.
    /// </summary>
    public DbSet<Project> Projects { get; set; }

    /// <summary>
    /// Роли для участников проекта.
    /// </summary>
    public DbSet<Role> ProjectRoles { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса TaskManagerDbContext с указанными параметрами.
    /// </summary>
    /// <param name="options">Опции для этого контекста.</param>
    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options) { }
}