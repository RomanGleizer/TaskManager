namespace Services.ViewModels.ProjectViewModels;

/// <summary>
/// Представление модели обновления проекта
/// </summary>
public record UpdateProjectViewModel
{
    /// <summary>
    /// Получает или задает идентификатор проекта
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Получает или задает имя проекта
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Получает или задает описание проекта
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Получает или задает дату последнего обновления проекта
    /// </summary>
    public required DateTime LastUpdatedDate { get; init; }

    /// <summary>
    /// Получает или задает список идентификаторов участников, связанных с проектом
    /// </summary>
    public required IList<Guid> MemberIds { get; init; }
}
