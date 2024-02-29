namespace Api.Controllers.Task.Requests;

/// <summary>
/// Представляет модель запроса для обновления задачи
/// </summary>
public class UpdateTaskRequest
{
    /// <summary>
    /// Получает или устанавливает название задачи
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Получает или устанавливает описание задачи
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Получает или устанавливает статус выполнения задачи
    /// </summary>
    public ExecutionStatus ExecutionStatus { get; set; }

    /// <summary>
    /// Получает или устанавливает дату создания задачи
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Получает или устанавливает дату последнего обновления задачи
    /// </summary>
    public DateTime LastUpdateDate { get; set; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов исполнителей, связанных с задачей
    /// </summary>
    public IList<string> PerformerIds { get; set; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов комментариев, связанных с задачей
    /// </summary>
    public IList<string> CommentIds { get; set; }
}
