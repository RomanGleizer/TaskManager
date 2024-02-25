using Dal.Interfaces;

namespace Dal.Entities;

/// <summary>
/// Сущность роли для базы данных
/// </summary>
public class RoleDal : IDbEntity<int>
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
