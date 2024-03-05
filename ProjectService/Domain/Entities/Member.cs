using Core.Dal.Base;

namespace Domain.Entities;

/// <summary>
/// Представляет информацию об участнике проекта
/// </summary>
public record Member : IBaseEntity<string>
{
    /// <summary>
    /// Получает или задает идентификатор участника проекта
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Получает или задает имя участника проекта
    /// </summary>
    public string FirstName { get; init; }

    /// <summary>
    /// Получает или задает фамилию участника проекта
    /// </summary>
    public string LastName { get; init; }

    /// <summary>
    /// Получает или задает электронную почту участника проекта
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    /// Получает или задает телефон участника проекта
    /// </summary>
    public string Phone { get; init; }

    /// <summary>
    /// Получает или задает идентификатор проекта, к которому принадлежит участник проекта
    /// </summary>
    public int ProjectId { get; init; }

    /// <summary>
    /// Получает или задает проект, к которому принадлежит участник проекта
    /// </summary>
    public Project Project { get; init; }

    /// <summary>
    /// Получает или задает идентификатор роли участника проекта
    /// </summary>
    public int RoleId { get; init; }

    /// <summary>
    /// Получает или задает роль участника проекта
    /// </summary>
    public Role Role { get; init; }
}
