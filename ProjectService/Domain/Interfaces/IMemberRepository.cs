using Domain.Entities;

namespace Domain.Interfaces;

public interface IMemberRepository
{
    Task<Member> GetMemberByIdAsync(string id);
}
