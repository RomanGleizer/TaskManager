namespace ConnectionLib.ConnectionServices.DtoModels.ProjectById;

/// <summary>
/// Представляет запрос API для получения информации о существующем проекте
/// </summary>
public record ExistingProjectApiRequest
{
    /// <summary>
    /// Получает или устанавливает идентификатор существующего проекта
    /// </summary>
    public required int ProjectId { get; init; }
}
