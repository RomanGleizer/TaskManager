namespace Api.Controllers.Task.Responses;

/// <summary>
/// Представляет модель ответа для создания задачи
/// </summary>
public class CreateTaskResponse
{
    /// <summary>
    /// Получает или инициализирует идентификатор созданной задачи
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Получает или инициализирует название задачи
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Получает или инициализирует описание задачи
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Получает или инициализирует статус выполнения задачи
    /// </summary>
    public ExecutionStatus ExecutionStatus { get; init; }

    /// <summary>
    /// Получает или инициализирует дату создания задачи
    /// </summary>
    public DateTime CreatedDate { get; init; }

    /// <summary>
    /// Получает или инициализирует дату последнего обновления задачи
    /// </summary>
    public DateTime LastUpdateDate { get; init; }

    /// <summary>
    /// Получает или инициализирует идентификатор проекта, к которому принадлежит задача
    /// </summary>
    public int ProjectId { get; init; }

    /// <summary>
    /// Получает или инициализирует перечисление идентификаторов исполнителей, связанных с задачей
    /// </summary>
    public IEnumerable<string> PerformerIds { get; init; }

    /// <summary>
    /// Получает или инициализирует список идентификаторов комментариев, связанных с задачей
    /// </summary>
    public IList<string> CommentIds { get; init; }
}
