using Core.Dal.Base;
using Microsoft.AspNetCore.Identity;

namespace Dal.Entities;

/// <summary>
/// Представляет сущность роли в базе данных
/// </summary>
public class RoleDal : IdentityRole, IBaseEntity<int>
{
    /// <summary>
    /// Получает или устанавливает идентификатор роли
    /// </summary>
    public required new int Id { get; init; }

    /// <summary>
    /// Получает или устанавливает название роли.
    /// </summary>
    public required new string Name { get; init; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов пользователей, связанных с данной ролью
    /// </summary>
    public required IList<string> UserIds { get; init; }
}
