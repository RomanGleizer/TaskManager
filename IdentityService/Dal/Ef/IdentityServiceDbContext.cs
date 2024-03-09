using Dal.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dal.Ef;

/// <summary>
/// Представляет контекст базы данных для службы идентификации
/// </summary>
public class IdentityServiceDbContext : IdentityDbContext<UserDal>
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
