using ProjectConnectionLib.ConnectionServices.DtoModels.PendingTaskNameLists;

namespace ProjectConnectionLib.ConnectionServices.Interfaces;

public interface ITaskConnectionService
{
    Task<PendingTaskListTaskApiResponse[]> GetUserNameListAsync(PendingTaskListTaskApiRequest request);
}
