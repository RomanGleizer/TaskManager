using Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Services.ViewModels.ProjectViewModels;

public record CreateProjectViewModel
{
    public CreateProjectViewModel()
    {
        CreationDate = DateTime.Now;
        LastUpdatedDate = DateTime.Now;
        MemberIds = new List<string>();
        Members = new List<Member>();
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    [SwaggerSchema(ReadOnly = true)]
    public DateTime CreationDate { get; init; }

    [SwaggerSchema(ReadOnly = true)]
    public DateTime LastUpdatedDate { get; init; }

    [SwaggerSchema(ReadOnly = true)]
    public IList<string> MemberIds { get; init; }

    [SwaggerSchema(ReadOnly = true)]
    public IList<Member> Members { get; init; }
}
