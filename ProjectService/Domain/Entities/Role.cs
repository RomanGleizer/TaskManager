using Core.Dal.Base;

namespace Domain.Entities;

/// <summary>
/// Представляет информацию о роли участника в проекте
/// </summary>
public record Role : IBaseEntity<int>
{
    /// <summary>
    /// Получает или задает идентификатор роли
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Получает или задает название роли
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Получает или задает список идентификаторов участников с этой ролью
    /// </summary>
    public required IList<string> MemberIds { get; init; }

    /// <summary>
    /// Получает или задает список участников с этой ролью
    /// </summary>
    public required IList<Member> Members { get; init; }
}
