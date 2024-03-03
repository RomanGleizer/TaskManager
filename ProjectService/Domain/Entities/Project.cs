namespace Domain.Entities;

// Должна быть унаследована от интерфейса BaseEntityDal, но он сейчас не доступен. (Исправить, когда будет доступен)
public record Project
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required DateTime CreationDate { get; init; }

    public required DateTime LastUpdatedDate { get; init; }

    public required IList<string> MemberIds { get; init; }

    public required IList<Member> Members { get; init; }
}
