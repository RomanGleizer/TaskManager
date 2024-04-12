using Core.Dal.Base;
using UsersMicroservice.UsersMicroserviceDal.Entities;
using UsersMicroservice.UsersMicroserviceDal.EntityFramework;
using UsersMicroservice.UsersMicroserviceDal.Interfaces;

namespace UsersMicroservice.UsersMicroserviceDal.Repositories;

/// <summary>
///     Представляет реализацию интерфейса IUnitOfWork на основе Entity Framework
/// </summary>
/// <remarks>
///     Инициализирует новый экземпляр класса EntityFrameworkUnitOfWork с указанным контекстом базы данных
/// </remarks>
/// <param name="context">Контекст базы данных Entity Framework</param>
public class EntityFrameworkUnitOfWork(UsersMicroserviceDbContext context) : IUnitOfWork
{
    private IRepository<RoleDal, int>? _roleRepository;

    /// <summary>
    ///     Получает репозиторий для работы с ролями
    /// </summary>
    public IRepository<RoleDal, int> Roles => _roleRepository ??= new RoleRepository(context);
}