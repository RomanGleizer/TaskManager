using Core.Dal.Base;
using Dal.Ef;
using Dal.Entities;
using Dal.Interfaces;

namespace Dal.Repositories;

/// <summary>
/// Представляет реализацию интерфейса IUnitOfWork на основе Entity Framework
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса EFUnitOfWork с указанным контекстом базы данных
/// </remarks>
/// <param name="context">Контекст базы данных Entity Framework</param>
public class EFUnitOfWork(IdentityServiceDbContext context) : IUnitOfWork
{
    private readonly IdentityServiceDbContext _context = context;
    private IRepository<RoleDal, int>? _roleRepository;

    /// <summary>
    /// Получает репозиторий для работы с ролями
    /// </summary>
    public IRepository<RoleDal, int> Roles
    {
        get
        {
            _roleRepository ??= new RoleRepository(_context);
            return _roleRepository;
        }
    }
}
