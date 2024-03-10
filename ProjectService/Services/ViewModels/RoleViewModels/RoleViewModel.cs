namespace Services.ViewModels.RoleViewModels;

/// <summary>
/// Представление модели роли
/// </summary>
public record RoleViewModel
{
    /// <summary>
    /// Получает или задает идентификатор роли
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Получает или задает имя роли
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Получает или задает список идентификаторов участников, связанных с этой ролью
    /// </summary>
    public IList<string> MemberIds { get; init; }
}