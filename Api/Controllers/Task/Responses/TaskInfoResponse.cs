namespace Api.Controllers.Task.Responses;

public class TaskInfoResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required ExecutionStatus ExecutionStatus { get; init; }

    public required DateTime CreatedDate { get; set; }

    public required DateTime LastUpdateDate { get; set; }

    public required IEnumerable<string> PerformerIds { get; set; }

    public required IList<string> CommentIds { get; set; }
}
