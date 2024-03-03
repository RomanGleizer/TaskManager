namespace Domain.Updated;

public record UpdateProjectData
{
    public required string Name { get; init; }

    public required string Description { get; init; }

    public required DateTime LastUpdatedDate { get; init; }

    public required IList<string> MemberIds { get; init; }
}
