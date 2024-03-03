namespace Api.Controllers.Project.Responses;

/// <summary>
/// Представляет модель ответа на запрос создания проекта
/// </summary>
public class CreateProjectResponse
{
    /// <summary>
    /// Получает или устанавливает идентификатор созданного проекта
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Получает или устанавливает название созданного проекта
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Получает или устанавливает описание созданного проекта
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Получает или устанавливает дату создания проекта
    /// </summary>
    public required DateTime CreationDate { get; init; }

    /// <summary>
    /// Получает или устанавливает дату последнего изменения проекта
    /// </summary>
    public required DateTime LastModifidedDate { get; init; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов участников проекта
    /// </summary>
    public required IList<string> ParticipantIds { get; init; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов задач, связанных с проектом
    /// </summary>
    public required IList<int> TaskIds { get; init; }
}
