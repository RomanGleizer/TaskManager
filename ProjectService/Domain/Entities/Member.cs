using Core.Dal.Base;

namespace Domain.Entities;

public record Member : IBaseEntity<string>
{
    public required string Id { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Email { get; init; }

    public required string Phone { get; init; }

    public required int ProjectId { get; init; }

    public required Project Project { get; init; }

    public required int RoleId { get; init; }

    public required Role Role { get; init; }
}
