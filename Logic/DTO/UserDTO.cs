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
    /// Дата создания пользователя
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Дата последнего изменения пользователя
    /// </summary>
    public DateTime LastModifidedDate { get; set; }

    /// <summary>
    /// Список идентификаторов участников
    /// </summary>
    public IList<string> ParticipantIds { get; set; }

    /// <summary>
    /// Список идентификаторов задач
    /// </summary>
    public IList<int> TaskIds { get; set; }
}
