using Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Services.ViewModels.ProjectViewModels;

/// <summary>
/// Представление модели создания проекта
/// </summary>
public record CreateProjectViewModel
{
    public CreateProjectViewModel()
    {
        CreationDate = DateTime.Now;
        LastUpdatedDate = DateTime.Now;
        MemberIds = new List<string>();
        Members = new List<Member>();
    }

    /// <summary>
    /// Получает или задает идентификатор проекта
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Получает или задает имя проекта
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Получает или задает описание проекта
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Получает или задает дату создания проекта
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public DateTime CreationDate { get; init; }

    /// <summary>
    /// Получает или задает дату последнего обновления проекта
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public DateTime LastUpdatedDate { get; init; }

    /// <summary>
    /// Получает или задает список идентификаторов участников, связанных с проектом
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public IList<string> MemberIds { get; init; }

    /// <summary>
    /// Получает или задает список участников проекта
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public IList<Member> Members { get; init; }
}
