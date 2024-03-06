using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using ProjectConnectionLib.ConnectionServices.DtoModels.PendingTaskNameLists;
using ProjectConnectionLib.ConnectionServices.Interfaces;
using Services.Interfaces;

namespace ProjectConnectionLib.ConnectionServices;

public class TaskConnectionService : ITaskConnectionService
{
    private readonly IHttpRequestService _httpRequestService;

    public TaskConnectionService(IHttpRequestService httpRequestService)
    {
        _httpRequestService = httpRequestService;
    }

    public async Task<TaskByIdApiResponse[]> GetPendingTaskListAsync(TaskByIdApiRequest request)
    {
        var endpoint = $"https://localhost:7047/api/Tasks/{request.TaskId}";
        var connectionData = new HttpConnectionData();

        var requestData = new HttpRequestData
        {
            Method = HttpMethod.Get,
            Uri = new Uri(endpoint)
        };

        var response = await _httpRequestService.SendRequestAsync<TaskByIdApiResponse[]>(requestData, connectionData);
        if (response.IsSuccessStatusCode)
        {
            return response.Body;
        }
        else
        {
            return Array.Empty<TaskByIdApiResponse>();
        }
    }
}
