namespace Services.ViewModels.MemberViewModels;

/// <summary>
/// Представление модели участника проекта
/// </summary>
public record MemberViewModel
{
    /// <summary>
    /// Получает или задает идентификатор участника проекта
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Получает или задает имя участника проекта
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// Получает или задает фамилию участника проекта
    /// </summary>
    public required string LastName { get; init; }
}
