using Domain.Entities;

namespace Services.Interfaces.MemberInterfaces;

public interface IFindProjectCreator
{
    Task FindProjectCreatorAsync(IList<Member> members);
}
