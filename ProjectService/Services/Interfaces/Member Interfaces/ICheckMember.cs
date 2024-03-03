using Domain.Entities;

namespace Services.Interfaces.MemberInterfaces;

public interface ICheckMember
{
    Task CheckMemberRegisterAsync(string memberId);
}
