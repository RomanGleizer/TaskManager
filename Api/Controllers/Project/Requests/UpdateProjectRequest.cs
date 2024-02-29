namespace Api.Controllers.Project.Requests;

/// <summary>
/// Модель запроса на обновление информации о проекте
/// </summary>
public class UpdateProjectRequest
{
    /// <summary>
    /// Получает или устанавливает идентификатор проекта
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Получает или устанавливает название проекта
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Получает или устанавливает описание проекта
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
