using Core.Dal.Base;

namespace Logic.Dto;

/// <summary>
/// Представляет объект передачи данных пользователя
/// </summary>
public record UserDTO : IBaseDTO<Guid>
{
    /// <summary>
    /// Получает или устанавливает идентификатор пользователя
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Получает или устанавливает имя пользователя
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// Получает или устанавливает фамилию пользователя
    /// </summary>
    public required string LastName { get; init; }

    /// <summary>
    /// Получает или устанавливает электронную почту пользователя
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Получает или устанавливает номер телефона пользователя
    /// </summary>
    public required string PhoneNumber { get; init; }

    /// <summary>
    /// Получает или устанавливает дату рождения пользователя
    /// </summary>
    public required DateTime BirthDay { get; init; }

    /// <summary>
    /// Получает или устанавливает идентификатор роли пользователя
    /// </summary>
    public required int RoleId { get; init; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов проектов, связанных с пользователем
    /// </summary>
    public required IList<int> ProjectIds { get; init; }
}