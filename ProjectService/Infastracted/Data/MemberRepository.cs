using Domain.Entities;
using Services.Interfaces.MemberInterfaces;

namespace Infastracted.Data;

public class MemberRepository : ICheckMember, IFindProjectCreator
{
    // Логика для работы с EF

    public Task CheckMemberRegisterAsync(string memberId)
    {
        throw new NotImplementedException();
    }

    public Task FindProjectCreatorAsync(IList<Member> members)
    {
        throw new NotImplementedException();
    }
}
