namespace Api.Controllers.Comment.Responses;

/// <summary>
/// Модель ответа при удалении комментария
/// </summary>
public class DeleteCommentResponse
{
    /// <summary>
    /// Получает или устанавливает идентификатор удаленного комментария
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Получает или устанавливает текст удаленного комментария
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Получает или устанавливает дату создания удаленного комментария
    /// </summary>
    public required DateTime CreatedDate { get; init; }

    /// <summary>
    /// Получает или устанавливает идентификатор автора удаленного комментария
    /// </summary>
    public required Guid AuthorId { get; init; }

    /// <summary>
    /// Получает или устанавливает идентификатор задачи, к которой относится удаленный комментарий
    /// </summary>
    public required int TaskId { get; init; }
}
