using Dal.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dal.Ef;

/// <summary>
/// Представляет контекст базы данных для службы идентификации
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса IdentityServiceDbContext с указанными параметрами
/// </remarks>
/// <param name="options">Опции для этого контекста</param>
public class IdentityServiceDbContext(DbContextOptions<IdentityServiceDbContext> options) : IdentityDbContext<UserDal>(options)
{
    /// <summary>
    /// Получает или устанавливает набор пользователей
    /// </summary>
    public override DbSet<UserDal> Users { get; set; }

    /// <summary>
    /// Получает или устанавливает набор ролей
    /// </summary>
    public new DbSet<RoleDal> Roles { get; set; }
}
