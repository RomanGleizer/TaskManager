using Core.Dal.Base;

namespace Logic.DTO;

/// <summary>
/// DTO для ProjectDal
/// </summary>
public class ProjectDTO : IBaseDTO<int>
{
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название проекта
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание проекта
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Дата создания проекта
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Последний момент обновления проекта
    /// </summary>
    public DateTime LastModifidedDate { get; set; }

    /// <summary>
    /// Уникальные идентификаторы участников проекта
    /// </summary>
    public IList<string> ParticipantIds { get; set; }

    /// <summary>
    /// Список идентификаторов задач в проекте
    /// </summary>
    public IList<int> TaskIds { get; set; }
}
