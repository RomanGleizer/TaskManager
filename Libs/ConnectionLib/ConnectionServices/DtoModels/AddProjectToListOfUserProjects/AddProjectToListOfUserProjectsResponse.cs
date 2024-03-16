using System.Text.Json.Serialization;

namespace ConnectionLib.ConnectionServices.DtoModels.AddProjectToListOfUserProjects;

public record AddProjectToListOfUserProjectsResponse
{
    /// <summary>
    /// Получает или устанавливает идентификатор проекта
    /// </summary>
    [JsonPropertyName("projectId")]
    public required int ProjectId { get; init; }

    /// <summary>
    /// Получает или устанавливает идентификатор участника
    /// </summary>
    [JsonPropertyName("memberId")]
    public required Guid MemberId { get; init; }
}