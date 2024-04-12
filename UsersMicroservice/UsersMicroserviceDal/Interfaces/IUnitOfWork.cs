using Core.Dal.Base;
using UsersMicroservice.UsersMicroserviceDal.Entities;

namespace UsersMicroservice.UsersMicroserviceDal.Interfaces;

/// <summary>
///     Представляет интерфейс для работы с единицей работы (UnitOfWork)
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///     Получает репозиторий для работы с ролями
    /// </summary>
    IRepository<RoleDal, int> Roles { get; }
}