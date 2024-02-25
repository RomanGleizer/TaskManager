using Core.Dal.Base;

namespace Dal.Entities;

/// <summary>
/// Сущность роли для базы данных
/// </summary>
public class RoleDal : IBaseEntityDal<int>
{
    /// <summary>
    /// Идентификатор роли
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название роли
    /// </summary>
    public string Name { get; set; }
}
