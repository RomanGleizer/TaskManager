using Microsoft.AspNetCore.Identity;

namespace Core.Dal.Base;

public interface IUserRepository<TDal>
    where TDal : IBaseEntity<Guid>
{
    /// <summary>
    /// Получает все сущности из репозитория асинхронно
    /// </summary>
    /// <returns>Список всех сущностей</returns>
    Task<IList<TDal>> GetAllAsync();

    /// <summary>
    /// Получает сущность из репозитория по идентификатору асинхронно
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Сущность с указанным идентификатором</returns>
    Task<TDal?> GetByIdAsync(Guid id);

    /// <summary>
    /// Создает новую сущность в репозитории асинхронно
    /// </summary>
    /// <param name="entity">Сущность для создания</param>
    /// <returns>Созданная сущность</returns>
    Task<IdentityResult?> CreateAsync(TDal entity, string password);

    /// <summary>
    /// Удаляет сущность из репозитория асинхронно
    /// </summary>
    /// <param name="entity">Сущность для удаления</param>
    /// <returns>Удаленная сущность</returns>
    Task<IdentityResult?> DeleteAsync(TDal entity);

    /// <summary>
    /// Обновляет сущность в репозитории асинхронно
    /// </summary>
    /// <param name="entity">Сущность для обновления</param>
    /// <returns>Обновленная сущность</returns>
    Task<IdentityResult?> UpdateAsync(TDal entity);
}
