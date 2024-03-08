using ConnectionLib.ConnectionServices.DtoModels.TaskById;

namespace ConnectionLib.ConnectionServices.Interfaces;

public interface ITaskConnectionService
{
    Task<ExistingTaskApiResponse> GetExistingTaskAsync(ExistingTaskApiRequest request);
}
