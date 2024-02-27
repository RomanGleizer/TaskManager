namespace Api.Controllers.Task.Responses;

public class UpdateTaskResponse
{
    public string Name { get; set; }

    public string Description { get; set; }

    public ExecutionStatus ExecutionStatus { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public IList<string> PerformerIds { get; set; }
}
