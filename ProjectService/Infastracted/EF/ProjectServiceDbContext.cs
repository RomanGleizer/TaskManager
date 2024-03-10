using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infastracted.EF;

/// <summary>
/// Контекст базы данных проекта, который обеспечивает доступ к таблицам проектов, членов и ролей
/// </summary>
public class ProjectServiceDbContext : DbContext
{
    /// <summary>
    /// Получает или задает набор данных для доступа к таблице проектов
    /// </summary>
    public DbSet<Project> Projects { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр контекста базы данных проекта
    /// </summary>
    /// <param name="options">Параметры конфигурации контекста базы данных</param>
    public ProjectServiceDbContext(DbContextOptions<ProjectServiceDbContext> options) : base(options) { }
}