namespace Services.ViewModels.RoleViewModels;

public record RoleViewModel
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required IList<string> MemberIds { get; init; }
}
