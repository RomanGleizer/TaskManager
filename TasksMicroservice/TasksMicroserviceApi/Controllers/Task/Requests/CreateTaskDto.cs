using TasksMicroservice.TasksMicroserviceDal.Enums;

namespace TasksMicroservice.TasksMicroserviceApi.Controllers.Task.Requests;

/// <summary>
///     Представляет модель запроса для создания задачи
/// </summary>
public record CreateTaskDto
{
    /// <summary>
    ///     Получает или устанавливает название задачи
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Получает или устанавливает описание задачи
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    ///     Получает или устанавливает список идентификаторов исполнителей, связанных с задачей
    /// </summary>
    public required IList<Guid> PerformerIds { get; init; }

    /// <summary>
    ///     Получает или устанавливает идентификатор проекта, к которому принадлежит задача
    /// </summary>
    public required Guid ProjectId { get; init; }

    /// <summary>
    ///     Получает или устанавливает идентификатор задачи
    /// </summary>
    public Guid Id => Guid.NewGuid();

    /// <summary>
    ///     Получает или устанавливает статус выполнения задачи
    /// </summary>
    public ExecutionStatus ExecutionStatus => ExecutionStatus.Pending;

    /// <summary>
    ///     Получает или устанавливает дату создания задачи
    /// </summary>
    public DateTime CreatedDate => DateTime.Now;

    /// <summary>
    ///     Получает или устанавливает дату последнего обновления задачи
    /// </summary>
    public DateTime LastUpdateDate => DateTime.Now;
}