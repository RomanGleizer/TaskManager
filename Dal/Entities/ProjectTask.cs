using Dal.Interfaces;

namespace Dal.Entities;

/// <summary>
/// Сущность задачи для базы данных
/// </summary>
public class ProjectTask : IDbEntity<int>
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
    public DateTime Created { get; set; }

    /// <summary>
    /// Дата последнего внесенного изменения
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// Постановщик задачи
    /// </summary>
    public User StageDirector { get; set; }

    /// <summary>
    /// Исполнители задачи
    /// </summary>
    public IEnumerable<User> Performers { get; set; }

    /// <summary>
    /// Идентификатор проекта, к которому относится задача 
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Ссылка на проект, к которому относится задача
    /// </summary>
    public Project Project { get; set; }
}
