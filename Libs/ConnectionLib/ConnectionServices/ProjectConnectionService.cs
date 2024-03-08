using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;
using ConnectionLib.ConnectionServices.Interfaces;

namespace ConnectionLib.ConnectionServices;

public class ProjectConnectionService : IProjectConnectionService
{
    private readonly IHttpRequestService _httpRequestService;
    private readonly string _baseUrl;

    public ProjectConnectionService(IServiceProvider serviceProvider)
    {
        _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
        _baseUrl = "https://localhost:7047/Projects";
    }

    public async Task<AddTaskInProjectApiResponse> AddTaskInProjectAsync(AddTaskInProjectApiRequest request)
    {
        var requestData = new HttpRequestData
        {
            Method = HttpMethod.Get,
            Uri = new Uri($"{_baseUrl}/{request.ProjectId}")
        };

        var connectionData = new HttpConnectionData();
        var response = await _httpRequestService.SendRequestAsync<AddTaskInProjectApiResponse>(requestData, connectionData);

        if (response.IsSuccessStatusCode)
        {
            // Добавление задачи
        }

        throw new Exception();
    }
}
