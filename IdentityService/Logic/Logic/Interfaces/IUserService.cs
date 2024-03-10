using Core.Dal.Base;
using Microsoft.AspNetCore.Identity;

namespace Logic.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с сущностями пользователей
/// </summary>
/// <typeparam name="TEntity">Тип сущности, представляющей пользовательские данные</typeparam>
/// <typeparam name="TId">Тип идентификатора пользователя</typeparam>
public interface IUserService<TEntity, TId>
    where TEntity : IBaseDTO<TId>
{
    /// <summary>
    /// Получает список пользователей по идентификатору
    /// </summary>
    /// <returns>Задача, представляющая асинхронную операцию. Результат задачи содержит список пользователей</returns>
    Task<IList<TEntity>> GetAllUsersAsync();

    /// <summary>
    /// Получает всех пользователей по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Задача, представляющая асинхронную операцию. Результат задачи содержит объект пользователя</returns>
    Task<TEntity> GetUserByIdAsync(TId id);

    /// <summary>
    /// Создает нового пользователя
    /// </summary>
    /// <param name="dto">Данные пользователя для создания</param>
    /// <returns>Задача, представляющая асинхронную операцию. Результат задачи содержит результат операции создания пользователя</returns>
    Task<IdentityResult> CreateUserAsync(TEntity dto);

    /// <summary>
    /// Удаляет пользователя по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя для удаления</param>
    /// <returns>Задача, представляющая асинхронную операцию. Результат задачи содержит результат операции удаления пользователя</returns>
    Task<IdentityResult> DeleteUserAsync(TId id);

    /// <summary>
    /// Обновляет данные пользователя
    /// </summary>
    /// <param name="dto">Новые данные пользователя</param>
    /// <param name="id">Идентификатор пользователя для обновления</param>
    /// <returns>Задача, представляющая асинхронную операцию. Результат задачи содержит результат операции обновления данных пользователя</returns>
    Task<IdentityResult> UpdateUserAsync(TEntity dto, TId id);
}
