namespace Api.Controllers.Task.Requests;

public class CreateTaskRequest
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required ExecutionStatus ExecutionStatus { get; set; }

    public required DateTime CreatedDate { get; set; }

    public required DateTime LastUpdateDate { get; set; }

    public required IList<string> PerformerIds { get; set; }

    public required int ProjectId { get; set; }
}
