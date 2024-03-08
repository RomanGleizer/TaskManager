namespace ConnectionLib.ConnectionServices.DtoModels.TaskById;

/// <summary>
/// Представляет запрос API для получения информации о существующей задаче
/// </summary>
public record ExistingTaskApiRequest
{
    /// <summary>
    /// Получает или устанавливает идентификатор существующей задачи
    /// </summary>
    public required int TaskId { get; init; }
}