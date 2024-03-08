namespace ConnectionLib.ConnectionServices.DtoModels.TaskById;

/// <summary>
/// Представляет ответ API с информацией о существующей задаче
/// </summary>
public record ExistingTaskApiResponse
{
    /// <summary>
    /// Получает или устанавливает значение, указывающее, существует ли задача
    /// </summary>
    public bool IsExists { get; set; }
}