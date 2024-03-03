using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreMember
{
    Task<Member> GetMemberByIdAsync(string id);
}
