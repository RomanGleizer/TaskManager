namespace Services.ViewModels.ProjectViewModels;

/// <summary>
/// Представление модели обновления проекта
/// </summary>
public record UpdateProjectViewModel
{
    /// <summary>
    /// Получает или задает идентификатор проекта
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Получает или задает имя проекта
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Получает или задает описание проекта
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Получает или задает дату последнего обновления проекта
    /// </summary>
    public DateTime LastUpdatedDate { get; init; }

    /// <summary>
    /// Получает или задает список идентификаторов участников, связанных с проектом
    /// </summary>
    public IList<string> MemberIds { get; init; }
}
