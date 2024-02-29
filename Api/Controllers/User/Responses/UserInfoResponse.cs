namespace Api.Controllers.User.Responses;

/// <summary>
/// Представляет информацию о пользователе
/// </summary>
public class UserInfoResponse
{
    /// <summary>
    /// Уникальный идентификатор пользователя
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    public required string LastName { get; init; }

    /// <summary>
    /// Адрес электронной почты пользователя
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Номер телефона пользователя
    /// </summary>
    public required string PhoneNumber { get; init; }

    /// <summary>
    /// День рождения пользователя
    /// </summary>
    public required DateTime BirthDay { get; init; }

    /// <summary>
    /// Уникальный идентификатор роли пользователя
    /// </summary>
    public required int RoleId { get; init; }
}
