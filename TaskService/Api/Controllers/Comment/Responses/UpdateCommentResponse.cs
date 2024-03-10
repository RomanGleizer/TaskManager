namespace Api.Controllers.Comment.Responses;

/// <summary>
/// Модель ответа при обновлении комментария
/// </summary>
public class UpdateCommentResponse
{
    /// <summary>
    /// Получает или устанавливает идентификатор обновленного комментария
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Получает или устанавливает текст обновленного комментария
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Получает или устанавливает дату создания обновленного комментария
    /// </summary>
    public required DateTime CreatedDate { get; init; }

    /// <summary>
    /// Получает или устанавливает идентификатор автора обновленного комментария
    /// </summary>
    public required Guid AuthorId { get; init; }

    /// <summary>
    /// Получает или устанавливает идентификатор задачи, к которой относится обновленный комментарий
    /// </summary>
    public required int TaskId { get; init; }
}
