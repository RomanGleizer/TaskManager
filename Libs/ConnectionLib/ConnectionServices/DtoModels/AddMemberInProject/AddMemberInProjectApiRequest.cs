namespace ConnectionLib.ConnectionServices.DtoModels.AddMemberInProject;

/// <summary>
/// Запрос на добавление участника в проект
/// </summary>
public record AddMemberInProjectApiRequest
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
