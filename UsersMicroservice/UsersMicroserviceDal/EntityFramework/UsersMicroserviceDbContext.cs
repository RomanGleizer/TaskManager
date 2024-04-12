using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsersMicroservice.UsersMicroserviceDal.Entities;

namespace UsersMicroservice.UsersMicroserviceDal.EntityFramework;

/// <summary>
///     Представляет контекст базы данных для службы идентификации
/// </summary>
/// <remarks>
///     Инициализирует новый экземпляр класса UsersMicroserviceDbContext с указанными параметрами
/// </remarks>
/// <param name="options">Опции для этого контекста</param>
public class UsersMicroserviceDbContext : IdentityDbContext<UserDal>
{
    public UsersMicroserviceDbContext(DbContextOptions<UsersMicroserviceDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    ///     Получает или устанавливает набор пользователей
    /// </summary>
    public override DbSet<UserDal> Users { get; set; }

    /// <summary>
    ///     Получает или устанавливает набор ролей
    /// </summary>
    public new DbSet<RoleDal> Roles { get; set; }
}