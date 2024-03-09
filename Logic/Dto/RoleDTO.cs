namespace Logic.Dto;

/// <summary>
/// Представляет объект передачи данных (DTO) роли
/// </summary>
public record RoleDTO
{
    /// <summary>
    /// Получает или устанавливает идентификатор роли
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Получает или устанавливает название роли
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов пользователей, связанных с данной ролью
    /// </summary>
    public required IList<string> UserIds { get; init; }
}