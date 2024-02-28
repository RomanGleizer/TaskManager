using Core.Dal.Base;

namespace Logic.DTO;

public class UserDTO : IBaseDTO<string>
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastModifidedDate { get; set; }

    public IList<string> ParticipantIds { get; set; }

    public IList<int> TaskIds { get; set; }
}
