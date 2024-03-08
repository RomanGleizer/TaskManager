namespace Services.ViewModels.ProjectViewModels;

/// <summary>
/// Представление модели проекта
/// </summary>
public record ProjectViewModel
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
    /// Получает или задает список идентификаторов участников, связанных с проектом
    /// </summary>
    public IList<string> MemberIds { get; init; }

    /// <summary>
    /// Получает или задает список идентификаторов задач, связанных с проектом
    /// </summary>
    public IList<int> TaskIds { get; init; }
}