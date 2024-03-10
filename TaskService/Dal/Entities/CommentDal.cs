using Core.Dal.Base;

namespace Dal.Entities;

/// <summary>
/// Представляет класс доступа к данным для управления комментариями
/// </summary>
public record CommentDal : IBaseEntity<int>
{
    /// <summary>
    /// Идентификатор комментария
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Текст комментария
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Дата создания комментария
    /// </summary>
    public required DateTime CreatedDate { get; init; }

    /// <summary>
    /// Уникальный идентификатор автора
    /// </summary>
    public required string AuthorId { get; init; }

    /// <summary>
    /// Уникальный идентификатор задачи
    /// </summary>
    public required int TaskId { get; init; }
}
