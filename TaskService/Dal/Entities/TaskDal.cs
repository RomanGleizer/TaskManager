using Core.Dal.Base;

namespace Dal.Entities;

/// <summary>
/// Сущность задачи для базы данных
/// </summary>
public record TaskDal : IBaseEntity<int>
{
    /// <summary>
    /// Идентификатор задачи
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Название задачи
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Описание задачи
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Текущий статус выполнения
    /// </summary>
    public required ExecutionStatus ExecutionStatus { get; init; }

    /// <summary>
    /// Дата создания задачи
    /// </summary>
    public required DateTime CreatedDate { get; init; }

    /// <summary>
    /// Дата последнего внесенного изменения
    /// </summary>
    public required DateTime LastUpdateDate { get; init; }

    /// <summary>
    /// Идентификаторы исполнителей задачи
    /// </summary>
    public required IList<Guid> PerformerIds { get; init; }

    /// <summary>
    /// Идентификаторы комментариев
    /// </summary>
    public required IList<int> CommentIds { get; init; }

    /// <summary>
    /// Комментарии, которые были добавлены к задаче
    /// </summary>
    public required IList<CommentDal> Comments { get; init; }

    /// <summary>
    /// Идентификатор проекта, к которому относится задача 
    /// </summary>
    public required int ProjectId { get; init; }
}
