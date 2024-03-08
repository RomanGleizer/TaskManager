namespace ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;

/// <summary>
/// Представляет запрос на добавление задачи в проект
/// </summary>
public record AddTaskInProjectApiRequest
{
    /// <summary>
    /// Получает или устанавливает идентификатор проекта, в который необходимо добавить задачу
    /// </summary>
    public required int ProjectId { get; init; }

    /// <summary>
    /// Получает или устанавливает идентификатор задачи, которую необходимо добавить в проект
    /// </summary>
    public required int TaskId { get; init; }
}
