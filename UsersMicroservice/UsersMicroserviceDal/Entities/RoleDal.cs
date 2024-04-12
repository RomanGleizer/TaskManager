using Core.Dal.Base;
using Microsoft.AspNetCore.Identity;

namespace UsersMicroservice.UsersMicroserviceDal.Entities;

/// <summary>
///     Представляет сущность роли в базе данных
/// </summary>
public class RoleDal : IdentityRole, IBaseEntity<int>
{
    /// <summary>
    ///     Получает или устанавливает название роли.
    /// </summary>
    public new required string Name { get; init; }

    /// <summary>
    ///     Получает или устанавливает список идентификаторов пользователей, связанных с данной ролью
    /// </summary>
    public required IList<string> UserIds { get; init; }

    /// <summary>
    ///     Получает или устанавливает идентификатор роли
    /// </summary>
    public new required int Id { get; init; }
}