using Core.Dal.Base;

namespace Domain.Entities;

/// <summary>
/// Представляет информацию о проекте
/// </summary>
public record Project : IBaseEntity<int>
{
    /// <summary>
    /// Получает или задает идентификатор проекта
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Получает или задает название проекта
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Получает или задает описание проекта
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Получает или задает дату создания проекта
    /// </summary>
    public required DateTime CreationDate { get; init; }

    /// <summary>
    /// Получает или задает дату последнего обновления проекта
    /// </summary>
    public required DateTime LastUpdatedDate { get; init; }

    /// <summary>
    /// Получает или задает список идентификаторов участников проекта
    /// </summary>
    public required IList<string> MemberIds { get; init; }

    /// <summary>
    /// Получает или задает список участников проекта
    /// </summary>
    public required IList<Member> Members { get; init; }

    /// <summary>
    /// Получает или задает список задач проекта
    /// </summary>
    public required IList<int> TaskIds { get; init; }
}
