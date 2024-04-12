using Core.Dal.Base;
using Microsoft.AspNetCore.Identity;

namespace UsersMicroservice.UsersMicroserviceDal.Entities;

/// <summary>
///     Представляет сущность пользователя в базе данных
/// </summary>
public class UserDal : IdentityUser, IBaseEntity<Guid>, IProjectIdsContainer
{
    /// <summary>
    ///     Получает или устанавливает имя пользователя
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    ///     Получает или устанавливает фамилию пользователя
    /// </summary>
    public required string LastName { get; init; }

    /// <summary>
    ///     Получает или устанавливает электронную почту пользователя
    /// </summary>
    public new required string Email { get; init; }

    /// <summary>
    ///     Получает или устанавливает номер телефона пользователя
    /// </summary>
    public new required string PhoneNumber { get; init; }

    /// <summary>
    ///     Получает или устанавливает дату рождения пользователя
    /// </summary>
    public required DateTime BirthDay { get; init; }

    /// <summary>
    ///     Получает или устанавливает идентификатор роли пользователя
    /// </summary>
    public required int RoleId { get; init; }

    /// <summary>
    ///     Возвращает или устанавливает имя пользователя, которое используется для аутентификации и идентификации пользователя
    ///     Для объекта UserDal возвращает значение свойства Email
    /// </summary>
    public override string UserName => Email;

    /// <summary>
    ///     Получает или устанавливает идентификатор пользователя
    /// </summary>
    public new required Guid Id { get; init; }

    /// <summary>
    ///     Получает или устанавливает список идентификаторов проектов, в которых состоит пользователь
    /// </summary>
    public required IList<Guid> ProjectIds { get; init; }
}