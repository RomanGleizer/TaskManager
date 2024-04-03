namespace Api.Controllers.Task.Responses;

/// <summary>
/// Представляет модель ответа для создания задачи
/// </summary>
public class CreateTaskResponse
{
    /// <summary>
    /// Получает или инициализирует идентификатор созданной задачи
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Получает или инициализирует название задачи
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Получает или инициализирует описание задачи
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Получает или инициализирует статус выполнения задачи
    /// </summary>
    public required ExecutionStatus ExecutionStatus { get; init; }

    /// <summary>
    /// Получает или инициализирует дату создания задачи
    /// </summary>
    public required DateTime CreatedDate { get; init; }

    /// <summary>
    /// Получает или инициализирует дату последнего обновления задачи
    /// </summary>
    public required DateTime LastUpdateDate { get; init; }

    /// <summary>
    /// Получает или инициализирует идентификатор проекта, к которому принадлежит задача
    /// </summary>
    public required Guid ProjectId { get; init; }

    /// <summary>
    /// Получает или инициализирует перечисление идентификаторов исполнителей, связанных с задачей
    /// </summary>
    public required IList<Guid> PerformerIds { get; init; }
}
