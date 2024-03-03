using Domain.Entities;

namespace ProjectApi.Controllers.Project_Requests;

public record CreateProjectRequest
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }
}
