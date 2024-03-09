namespace Logic.Dto.User;

public record CreateUserDTO
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
    /// Получает или устанавливает пароль пользователя
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// Получает или устанавливает идентификатор роли пользователя
    /// </summary>
    public required int RoleId { get; init; }

    /// <summary>
    /// Получает или устанавливает список идентификаторов проектов, в которых состоит пользователь
    /// </summary>
    public required IList<int> ProjectIds { get; init; }
}
