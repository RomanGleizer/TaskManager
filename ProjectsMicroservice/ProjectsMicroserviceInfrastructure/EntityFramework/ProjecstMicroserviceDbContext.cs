using Microsoft.EntityFrameworkCore;
using ProjectsMicroservice.ProjectsMicroserviceDomain.Entities;

namespace ProjectsMicroservice.ProjectsMicroserviceInfrastructure.EntityFramework;

/// <summary>
///     Контекст базы данных проекта, который обеспечивает доступ к таблицам проектов, членов и ролей
/// </summary>
public class ProjecstMicroserviceDbContext : DbContext
{
    /// <summary>
    ///     Инициализирует новый экземпляр контекста базы данных проекта
    /// </summary>
    /// <param name="options">Параметры конфигурации контекста базы данных</param>
    public ProjecstMicroserviceDbContext(DbContextOptions<ProjecstMicroserviceDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    ///     Получает или задает набор данных для доступа к таблице проектов
    /// </summary>
    public DbSet<Project> Projects { get; set; }
}