namespace Api.Controllers.User.Requests;

/// <summary>
/// Представляет информацию о запросе на создание пользователя
/// </summary>
public class CreateUserRequest
{
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
