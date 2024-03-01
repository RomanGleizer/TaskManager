using Core.Dal.Base;

namespace Logic.DTO;

/// <summary>
/// DTO объект пользователя
/// </summary>
public class UserDTO : IBaseDTO<string>
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Электронная почта пользователя
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Номер телефона пользователя
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// День рождения пользователя
    /// </summary>
    public DateTime BirthDay { get; set; }

    /// <summary>
    /// Уникальный идентификатор роли
    /// </summary>
    public int RoleId { get; set; }
}
