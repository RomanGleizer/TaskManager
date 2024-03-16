namespace ConnectionLib.ConnectionServices.DtoModels.AddProjectToListOfUserProjects;

/// <summary>
/// Запрос на добавление участника в проект
/// </summary>
public record AddProjectToListOfUserProjectsRequest
{
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public required int ProjectId { get; init; }

    /// <summary>
    /// Идентификатор участника
    /// </summary>
    public required Guid MemberId { get; init; }
}
