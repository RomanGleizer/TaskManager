using Core.Dal.Base;

namespace Logic.DTO;

/// <summary>
/// DTO объект комментария
/// </summary>
public class CommentDTO : IBaseDTO<int>
{
    /// <summary>
    /// Идентификатор комментария
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Текст комментария
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Дата создания комментария
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Идентификатор автора комментария
    /// </summary>
    public string AuthorId { get; set; }

    /// <summary>
    /// Идентификатор связанной задачи
    /// </summary>
    public int TaskId { get; set; }
}
