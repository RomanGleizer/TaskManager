namespace Services.ViewModels;

public record ProjectViewModel
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required IList<string> MemberIds { get; init; }
}
