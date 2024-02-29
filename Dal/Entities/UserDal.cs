using Microsoft.AspNetCore.Identity;
using Core.Dal.Base;

namespace Dal.Entities;

/*
 * Важное замечание. Запланирован отдельный сервис, который  будет работать с пользователем. Это временная dal модель
*/

/// <summary>
/// Сущность пользователя для бд
/// </summary>
public class UserDal : IdentityUser, IBaseEntityDal<string>
{
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
    public override string Email { get; set; }

    /// <summary>
    /// Номер телефона пользователя
    /// </summary>
    public override string PhoneNumber { get; set; }

    /// <summary>
    /// День рождения пользователя
    /// </summary>
    public DateTime BirthDay { get; set; }

    /// <summary>
    /// Уникальный идентификатор роли
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Роль пользователя в проекте
    /// </summary>
    public RoleDal Role { get; set; }
}