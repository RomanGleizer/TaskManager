using ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;

namespace ConnectionLib.ConnectionServices.Interfaces;

public interface IProjectConnectionService
{
    Task<AddTaskInProjectApiResponse> AddTaskInProjectAsync(AddTaskInProjectApiRequest request);
}
