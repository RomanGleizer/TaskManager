using Core.Dal.Base;

namespace Logic.DTO;

/// <summary>
/// DTO для представления информации о роли
/// </summary>
public class RoleDTO : IBaseDTO<int>
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
