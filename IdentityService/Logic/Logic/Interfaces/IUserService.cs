using Logic.Dto.User;
using Microsoft.AspNetCore.Identity;

namespace Logic.Interfaces;

/// <summary>
/// Представляет интерфейс сервиса пользователей.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Получает все модели представления пользователей асинхронно
    /// </summary>
    /// <returns>Список всех моделей представления пользователей</returns>
    Task<IList<UserDto>> GetAllUsersAsync();

    /// <summary>
    /// Получает модель представления пользователя по указанному идентификатору асинхронно
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Модель представления пользователя с указанным идентификатором</returns>
    Task<UserDto> GetUserByIdAsync(Guid id);

    /// <summary>
    /// Создает нового пользователя
    /// </summary>
    /// <param name="user">Модель представления нового пользователя</param>
    /// <returns>Модель представления созданного пользователя</returns>
    Task<IdentityResult> CreateUserAsync(CreateUserDto user, string password);

    /// <summary>
    /// Удаляет пользователя по указанному идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    Task<IdentityResult> DeleteUserAsync(Guid id);

    /// <summary>
    /// Обновляет информацию о пользователе
    /// </summary>
    /// <param name="user">Модель представления пользователя с обновленной информацией</param>
    Task<IdentityResult> UpdateUserAsync(Guid id, UpdateUserDto user);

    /// <summary>
    /// Добавляет новый проект у пользователя
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="memberId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<IdentityResult> AddNewProject(int projectId, Guid memberId);
}
