using Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Services.ViewModels;

public record CreateProjectViewModel
{
    public CreateProjectViewModel()
    {
        CreationDate = DateTime.Now;
        LastUpdatedDate = DateTime.Now;
        MemberIds = new List<string>();
        Members = new List<Member>();
    }

    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    [SwaggerSchema(ReadOnly = true)]
    public required DateTime CreationDate { get; init; }

    [SwaggerSchema(ReadOnly = true)]
    public required DateTime LastUpdatedDate { get; init; }

    [SwaggerSchema(ReadOnly = true)]
    public required IList<string> MemberIds { get; init; }

    [SwaggerSchema(ReadOnly = true)]
    public required IList<Member> Members { get; init; }
}
