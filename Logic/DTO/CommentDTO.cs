using Core.Dal.Base;

namespace Logic.DTO;

public class CommentDTO : IBaseDTO<int>
{
    public int Id { get; set; }

    public string Text { get; set; }

    public DateTime CreatedDate { get; set; }

    public string AuthorId { get; set; }

    public int TaskId { get; set; }
}
