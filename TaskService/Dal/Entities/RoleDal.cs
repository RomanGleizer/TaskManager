using Core.Dal.Base;

namespace Dal.Entities;

/// <summary>
/// Сущность роли для базы данных
/// </summary>
public class RoleDal : IBaseEntity<int>
{
    /// <summary>
    /// Идентификатор роли
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Название роли
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Уникальные идентификаторы участников с ролью
    /// </summary>
    public IList<string> UserIds { get; set; }

    /// <summary>
    /// Пользователи с ролью
    /// </summary>
    public IList<UserDal> Users { get; set; }
}
