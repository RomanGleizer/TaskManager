namespace Services.ViewModels.ProjectViewModels;

public record UpdateProjectViewModel
{
    public int Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public DateTime LastUpdatedDate { get; init; }

    public IList<string> MemberIds { get; init; }
}
