namespace ConnectionLibrary.ConnectionServices.DtoModels.ExistingTaskInProject;

/// <summary>
///     Представляет ответ API с информацией о существующей задаче в проекте
/// </summary>
public record ExistingTaskInProjectResponse
{
    /// <summary>
    ///     Получает или устанавливает значение cписка с идентификаторами задач
    /// </summary>
    public required IList<Guid> TaskIds { get; init; }
}