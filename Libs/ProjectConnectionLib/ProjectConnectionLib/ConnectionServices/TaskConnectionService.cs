using ProjectConnectionLib.ConnectionServices.DtoModels.PendingTaskNameLists;
using ProjectConnectionLib.ConnectionServices.Interfaces;

namespace ProjectConnectionLib.ConnectionServices;

public class TaskConnectionService : ITaskConnectionService
{
    public Task<PendingTaskListTaskApiResponse[]> GetUserNameListAsync(PendingTaskListTaskApiRequest request)
    {
        throw new NotImplementedException();
    }
}
