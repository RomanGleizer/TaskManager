using Core.Dal.Base;
using Dal.Ef;
using Dal.Entities;
using Dal.Interfaces;

namespace Dal.Repositories;

/// <summary>
/// Представляет реализацию интерфейса IUnitOfWork на основе Entity Framework
/// </summary>
public class EFUnitOfWork : IUnitOfWork
{
    private readonly IdentityServiceDbContext _context;
    private IRepository<RoleDal, int>? _roleRepository;

    /// <summary>
    /// Инициализирует новый экземпляр класса EFUnitOfWork с указанным контекстом базы данных
    /// </summary>
    /// <param name="context">Контекст базы данных Entity Framework</param>
    public EFUnitOfWork(IdentityServiceDbContext context)
    {
        _context = context;
    }

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
