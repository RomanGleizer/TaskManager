﻿namespace Api.Controllers.Role.Requests;

/// <summary>
/// Представляет запрос на обновление роли
/// </summary>
public record UpdateRoleDTO
{
    /// <summary>
    /// Уникальный идентификатор роли
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Наименование роли
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Список уникальных идентификаторов пользователей, связанных с данной ролью
    /// </summary>
    public required IList<string> UserIds { get; init; }
}