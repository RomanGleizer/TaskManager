using Core.Dal.Base;
using TasksMicroservice.TasksMicroserviceDal.Enums;

namespace TasksMicroservice.TasksMicroserviceLogic.Dto;

/// <summary>
///     DTO объект задачи
/// </summary>
public record TaskDto : IBaseDto<Guid>
{
    /// <summary>
    ///     Название задачи
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Описание задачи
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    ///     Текущий статус выполнения
    /// </summary>
    public required ExecutionStatus ExecutionStatus { get; init; }

    /// <summary>
    ///     Дата создания задачи
    /// </summary>
    public required DateTime CreatedDate { get; init; }

    /// <summary>
    ///     Дата последнего внесенного изменения
    /// </summary>
    public required DateTime LastUpdateDate { get; init; }

    /// <summary>
    ///     Идентификаторы исполнителей задачи
    /// </summary>
    public required IList<Guid> PerformerIds { get; init; }

    /// <summary>
    ///     Идентификатор проекта, к которому относится задача
    /// </summary>
    public required Guid ProjectId { get; init; }

    /// <summary>
    ///     Идентификатор задачи
    /// </summary>
    public required Guid Id { get; init; }
}