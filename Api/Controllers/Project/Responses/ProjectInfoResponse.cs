namespace Api.Controllers.Project.Responses;

public class ProjectInfoResponse
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required DateTime CreationDate { get; set; }

    public required DateTime LastModifidedDate { get; set; }

    public required IList<string> ParticipantIds { get; set; }

    public required IList<int> TaskIds { get; set; }
}
