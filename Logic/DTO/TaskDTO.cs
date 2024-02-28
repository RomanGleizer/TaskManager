using Core.Dal.Base;

namespace Logic.DTO;

/// <summary>
/// DTO для TaskDal
/// </summary>
public class TaskDTO : IBaseDTO<int>
{
    /// <summary>
    /// Идентификатор задачи
    /// </summary>
    public int Id { get; set; }

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
    /// Идентификатор постановщика задачи
    /// </summary>
    public string StageDirectorId { get; set; }

    /// <summary>
    /// Идентификаторы исполнителей задачи
    /// </summary>
    public IList<string> PerformerIds { get; set; }

    /// <summary>
    /// Идентификаторы комментариев
    /// </summary>
    public IList<string> CommentIds { get; set; }

    /// <summary>
    /// Идентификатор проекта, к которому относится задача
    /// </summary>
    public int? ProjectId { get; set; }
}
