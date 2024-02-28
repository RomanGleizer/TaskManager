namespace Api.Controllers.Task.Responses;

public class CreateTaskResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required ExecutionStatus ExecutionStatus { get; init; }

    public required DateTime CreatedDate { get; init; }

    public required DateTime LastUpdateDate { get; init; }

    public required int? ProjectId { get; init; }

    public required IEnumerable<string> PerformerIds { get; init; }

    public required IList<string> CommentIds { get; set; }
}
