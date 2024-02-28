using Dal.Entities;

namespace Api.Controllers.Project.Requests;

public class AddNewParticipantRequest
{
    public required UserDal NewParticipant { get; set; }
}
