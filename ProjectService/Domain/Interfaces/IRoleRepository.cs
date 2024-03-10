using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Интерфейс для работы с данными ролей
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Получает роль по её идентификатору
    /// </summary>
    /// <param name="roleId">Идентификатор роли</param>
    /// <returns>Объект роли или null, если роль не найдена</returns>
    Task<Role?> GetRoleByIdAsync(int roleId);
}
