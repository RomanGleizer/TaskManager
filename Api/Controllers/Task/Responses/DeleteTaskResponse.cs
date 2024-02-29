namespace Api.Controllers.Task.Responses;

/// <summary>
/// Представляет модель ответа для удаления задачи
/// </summary>
public class DeleteTaskResponse
{
    /// <summary>
    /// Получает или инициализирует название удаленной задачи
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Получает или инициализирует описание удаленной задачи
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Получает или инициализирует статус выполнения удаленной задачи
    /// </summary>
    public ExecutionStatus ExecutionStatus { get; init; }

    /// <summary>
    /// Получает или инициализирует дату создания удаленной задачи
    /// </summary>
    public DateTime CreatedDate { get; init; }

    /// <summary>
    /// Получает или инициализирует дату последнего обновления удаленной задачи
    /// </summary>
    public DateTime LastUpdateDate { get; init; }

    /// <summary>
    /// Получает или инициализирует список идентификаторов исполнителей, связанных с удаленной задачей
    /// </summary>
    public IList<string> PerformerIds { get; init; }

    /// <summary>
    /// Получает или инициализирует список идентификаторов комментариев, связанных с удаленной задачей
    /// </summary>
    public IList<string> CommentIds { get; init; }

    /// <summary>
    /// Получает или инициализирует идентификатор проекта, к которому принадлежит удаленная задача
    /// </summary>
    public int ProjectId { get; init; }
}
