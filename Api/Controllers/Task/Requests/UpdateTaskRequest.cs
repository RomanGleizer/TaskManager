namespace Api.Controllers.Task.Requests;

/// <summary>
/// Представляет модель запроса для обновления задачи
/// </summary>
public class UpdateTaskRequest
{
    /// <summary>
    /// Получает или устанавливает название задачи
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Получает или устанавливает описание задачи
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Получает или устанавливает статус выполнения задачи
    /// </summary>
    public required ExecutionStatus ExecutionStatus { get; set; }

    /// <summary>
    /// Получает или устанавливает дату создания задачи
    /// </summary>
    public required DateTime CreatedDate { get; set; }

    /// <summary>
    /// Получает или устанавливает дату последнего обновления задачи
    /// </summary>
    public required DateTime LastUpdateDate { get; set; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов исполнителей, связанных с задачей
    /// </summary>
    public required IList<string> PerformerIds { get; set; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов комментариев, связанных с задачей
    /// </summary>
    public required IList<string> CommentIds { get; set; }
}
