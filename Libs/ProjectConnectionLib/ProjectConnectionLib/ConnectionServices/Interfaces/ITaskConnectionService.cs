using ProjectConnectionLib.ConnectionServices.DtoModels.PendingTaskNameLists;

namespace ProjectConnectionLib.ConnectionServices.Interfaces;

public interface ITaskConnectionService
{
    Task<TaskByIdApiResponse[]> GetPendingTaskListAsync(TaskByIdApiRequest request);
}
