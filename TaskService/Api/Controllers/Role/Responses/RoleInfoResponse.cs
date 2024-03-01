namespace Api.Controllers.Role.Responses;

/// <summary>
/// Представляет информацию о роли, про которую был сделан запрос
/// </summary>
public class RoleInfoResponse
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
