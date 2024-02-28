namespace Api.Controllers.Project.Responses;

public class DeleteProjectResponse
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastModifidedDate { get; set; }

    public IList<string> ParticipantIds { get; set; }

    public IList<int> TaskIds { get; set; }
}
