namespace ProjectsMicroservice.ProjectsMicroserviceApplication.ViewModels.ProjectViewModels;

/// <summary>
///     Представление модели создания проекта
/// </summary>
public record CreateProjectViewModel
{
    /// <summary>
    ///     Получает или задает имя проекта
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Получает или задает описание проекта
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    ///     Получает или задает список идентификаторов участников, связанных с проектом
    /// </summary>
    public required IList<Guid> MemberIds { get; init; }

    /// <summary>
    ///     Получает или задает идентификатор проекта
    /// </summary>
    public Guid Id => Guid.NewGuid();

    /// <summary>
    ///     Получает или задает дату создания проекта
    /// </summary>
    public DateTime CreationDate => DateTime.Now;

    /// <summary>
    ///     Получает или задает дату последнего обновления проекта
    /// </summary>
    public DateTime LastUpdatedDate => DateTime.Now;

    /// <summary>
    ///     Получает или задает список идентификаторов задач, связанных с проектом
    /// </summary>
    public IList<Guid> TaskIds => [];
}