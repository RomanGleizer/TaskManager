using Core.Dal.Base;

namespace Dal.Entities;

/// <summary>
/// Сущность задачи для базы данных
/// </summary>
public class TaskDal : IBaseEntity<int>
{
    /// <summary>
    /// Идентификатор задачи
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Название задачи
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание задачи
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Текущий статус выполнения
    /// </summary>
    public ExecutionStatus ExecutionStatus { get; set; }

    /// <summary>
    /// Дата создания задачи
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Дата последнего внесенного изменения
    /// </summary>
    public DateTime LastUpdateDate { get; set; }

    /// <summary>
    /// Идентификаторы исполнителей задачи
    /// </summary>
    public IList<string> PerformerIds { get; set; }

    /// <summary>
    /// Идентификаторы комментариев
    /// </summary>
    public IList<int> CommentIds { get; set; }

    /// <summary>
    /// Комментарии, которые были добавлены к задаче
    /// </summary>
    public IList<CommentDal> Comments { get; set; }

    /// <summary>
    /// Идентификатор проекта, к которому относится задача 
    /// </summary>
    public int ProjectId { get; set; }
}
