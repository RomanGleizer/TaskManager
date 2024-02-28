namespace Api.Controllers.Comment.Requests;

public class CreateCommentRequest
{
    public required int Id { get; set; }

    public required string Text { get; set; }

    public required DateTime CreatedDate { get; set; }

    public required string AuthorId { get; set; }

    public required int TaskId { get; set; }
}
