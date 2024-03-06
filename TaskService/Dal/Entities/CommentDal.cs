using Core.Dal.Base;

namespace Dal.Entities;

/// <summary>
/// Представляет класс доступа к данным для управления комментариями
/// </summary>
public class CommentDal : IBaseEntity<int>
{
    /// <summary>
    /// Идентификатор комментария
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Текст комментария
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Дата создания комментария
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Уникальный идентификатор автора
    /// </summary>
    public string AuthorId { get; set; }

    /// <summary>
    /// Автор комментария
    /// </summary>
    public UserDal Author { get; set; }

    /// <summary>
    /// Уникальный идентификатор задачи
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Задание, к которому относится комментарий
    /// </summary>
    public TaskDal Task { get; set; }
}
