﻿namespace Api.Controllers.Role.Responses;

/// <summary>
/// Представляет информацию о обновленной роли
/// </summary>
public class UpdateRoleResponse
{
    /// <summary>
    /// Уникальный идентификатор роли
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование роли
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Список уникальных идентификаторов пользователей, связанных с данной ролью
    /// </summary>
    public IList<string> UserIds { get; set; }
}
