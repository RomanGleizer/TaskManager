﻿using Core.Dal.Base;

namespace Domain.Entities;

public record Role : IBaseEntity<int>
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required IList<string> MemberIds { get; init; }

    public required IList<Member> Members { get; init; }
}
