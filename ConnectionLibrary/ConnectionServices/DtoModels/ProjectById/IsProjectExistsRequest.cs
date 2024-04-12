namespace ConnectionLibrary.ConnectionServices.DtoModels.ProjectById;

/// <summary>
///     Представляет запрос API для получения информации о существующем проекте
/// </summary>
public record IsProjectExistsRequest
{
    /// <summary>
    ///     Получает или устанавливает идентификатор существующего проекта
    /// </summary>
    public required Guid ProjectId { get; init; }
}