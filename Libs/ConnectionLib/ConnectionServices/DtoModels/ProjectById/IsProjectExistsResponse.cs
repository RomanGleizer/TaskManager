using System.Text.Json.Serialization;

namespace ConnectionLib.ConnectionServices.DtoModels.ProjectById;

/// <summary>
/// Представляет ответ API с информацией о существующем проекте
/// </summary>
public record IsProjectExistsResponse
{
    /// <summary>
    /// Получает или устанавливает идентификатор проекта
    /// </summary>
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    /// <summary>
    /// Получает или устанавливает имя проекта
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Получает или устанавливает дату создания проекта
    /// </summary>
    [JsonPropertyName("creationDate")]
    public required DateTime CreationDate { get; init; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов задач, связанных с проектом
    /// </summary>
    [JsonPropertyName("taskIds")]
    public required IList<int> TaskIds { get; init; }
}
