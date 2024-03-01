namespace Api.Controllers.Project.Responses;

/// <summary>
/// Представляет модель ответа на запрос удаления проекта
/// </summary>
public class DeleteProjectResponse
{
    /// <summary>
    /// Получает или устанавливает идентификатор удаленного проекта
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Получает или устанавливает название удаленного проекта
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Получает или устанавливает описание удаленного проекта
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Получает или устанавливает дату создания удаленного проекта
    /// </summary>
    public required DateTime CreationDate { get; init; }

    /// <summary>
    /// Получает или устанавливает дату последнего изменения удаленного проекта
    /// </summary>
    public required DateTime LastModifidedDate { get; init; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов участников удаленного проекта
    /// </summary>
    public required IList<string> ParticipantIds { get; init; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов задач, связанных с удаленным проектом
    /// </summary>
    public required IList<int> TaskIds { get; init; }
}
