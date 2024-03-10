namespace Api.Controllers.Comment.Responses;

/// <summary>
/// Модель ответа при создании комментария
/// </summary>
public class CreateCommentResponse
{
    /// <summary>
    /// Получает или устанавливает идентификатор созданного комментария
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Получает или устанавливает текст созданного комментария
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Получает или устанавливает дату создания созданного комментария
    /// </summary>
    public required DateTime CreatedDate { get; init; }

    /// <summary>
    /// Получает или устанавливает идентификатор автора созданного комментария
    /// </summary>
    public required Guid AuthorId { get; init; }

    /// <summary>
    /// Получает или устанавливает идентификатор задачи, к которой относится созданный комментарий
    /// </summary>
    public required int TaskId { get; init; }
}
