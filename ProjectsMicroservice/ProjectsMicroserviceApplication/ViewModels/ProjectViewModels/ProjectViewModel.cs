namespace ProjectsMicroservice.ProjectsMicroserviceApplication.ViewModels.ProjectViewModels;

/// <summary>
///     Представление модели проекта
/// </summary>
public record ProjectViewModel
{
    /// <summary>
    ///     Получает или задает идентификатор проекта
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    ///     Получает или задает имя проекта
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Получает или задает список идентификаторов участников, связанных с проектом
    /// </summary>
    public required IList<Guid> MemberIds { get; init; }

    /// <summary>
    ///     Получает или задает список идентификаторов задач, связанных с проектом
    /// </summary>
    public required IList<int> TaskIds { get; init; }
}