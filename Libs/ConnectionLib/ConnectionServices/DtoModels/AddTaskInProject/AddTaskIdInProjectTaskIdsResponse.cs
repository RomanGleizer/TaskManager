using System.Text.Json.Serialization;

namespace ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;

/// <summary>
/// Представляет ответ API на запрос добавления задачи в проект
/// </summary>
public record AddTaskIdInProjectTaskIdsResponse
{
    /// <summary>
    /// Получает или устанавливает идентификатор проекта
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid ProjectId { get; init; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов задач
    /// </summary>
    [JsonPropertyName("taskIds")]
    public required IList<Guid> TaskIds { get; init; }
}
