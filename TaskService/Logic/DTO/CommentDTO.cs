using Core.Dal.Base;

namespace Logic.DTO;

/// <summary>
/// DTO объект комментария
/// </summary>
public class CommentDTO : IBaseDTO<int>
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
    /// Идентификатор автора комментария
    /// </summary>
    public required string AuthorId { get; init; }

    /// <summary>
    /// Идентификатор связанной задачи
    /// </summary>
    public required int TaskId { get; init; }
}
