namespace ConnectionLibrary.ConnectionServices.DtoModels.ExistingTaskInProject;

/// <summary>
///     Представляет запрос API для получения информации о существующей задаче
/// </summary>
public record ExistingTaskInProjectRequest
{
    /// <summary>
    ///     Получает или устанавливает идентификатор существующей задачи
    /// </summary>
    public required Guid ProjectId { get; init; }

    /// <summary>
    ///     Получает или устанавливает идентификатор существующей задачи
    /// </summary>
    public required Guid TaskId { get; init; }
}