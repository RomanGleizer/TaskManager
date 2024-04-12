using Microsoft.AspNetCore.Identity;
using UsersMicroservice.UsersMicroserviceLogic.Dto.User;

namespace UsersMicroservice.UsersMicroserviceLogic.Interfaces;

/// <summary>
///     Представляет интерфейс сервиса пользователей.
/// </summary>
public interface IUserService
{
    /// <summary>
    ///     Получает все модели представления пользователей асинхронно
    /// </summary>
    /// <returns>Список всех моделей представления пользователей</returns>
    Task<IList<UserDto>> GetAllUsersAsync();

    /// <summary>
    ///     Получает модель представления пользователя по указанному идентификатору асинхронно
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Модель представления пользователя с указанным идентификатором</returns>
    Task<UserDto> GetUserByIdAsync(Guid id);

    /// <summary>
    ///     Создает нового пользователя
    /// </summary>
    /// <param name="user">Модель представления нового пользователя</param>
    /// <param name="password">Пароль пользователя</param>
    /// <returns>Модель представления созданного пользователя</returns>
    Task<IdentityResult> CreateUserAsync(CreateUserDto user, string password);

    /// <summary>
    ///     Удаляет пользователя по указанному идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    Task<IdentityResult> DeleteUserAsync(Guid id);

    /// <summary>
    ///     Обновляет информацию о пользователе
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <param name="user">Модель представления пользователя с обновленной информацией</param>
    Task<IdentityResult> UpdateUserAsync(Guid id, UpdateUserDto user);
}