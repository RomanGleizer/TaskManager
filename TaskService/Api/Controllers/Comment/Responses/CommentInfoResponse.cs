namespace Api.Controllers.Comment.Responses;

/// <summary>
/// Модель ответа с информацией о комментарии
/// </summary>
public class CommentInfoResponse
{
    /// <summary>
    /// Получает или устанавливает идентификатор комментария
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Получает или устанавливает текст комментария
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Получает или устанавливает дату создания комментария
    /// </summary>
    public required DateTime CreatedDate { get; init; }

    /// <summary>
    /// Получает или устанавливает идентификатор автора комментария
    /// </summary>
    public required Guid AuthorId { get; init; }

    /// <summary>
    /// Получает или устанавливает идентификатор задачи, к которой относится комментарий
    /// </summary>
    public required int TaskId { get; init; }
}
