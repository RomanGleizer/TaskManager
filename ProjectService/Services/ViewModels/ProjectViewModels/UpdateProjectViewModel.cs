namespace Services.ViewModels.ProjectViewModels;

public record UpdateProjectViewModel
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required DateTime LastUpdatedDate { get; init; }

    public required IList<string> MemberIds { get; init; }
}
