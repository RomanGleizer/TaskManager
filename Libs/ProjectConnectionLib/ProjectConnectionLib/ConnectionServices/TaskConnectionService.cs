using Core.Exceptions;
using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ProjectConnectionLib.ConnectionServices.DtoModels.PendingTaskNameLists;
using ProjectConnectionLib.ConnectionServices.Interfaces;
using System.Net;

namespace ProjectConnectionLib.ConnectionServices;

public class TaskConnectionService : ITaskConnectionService
{
    private readonly IHttpRequestService _httpRequestService;
    private readonly string _baseUrl;

    public TaskConnectionService(IServiceProvider serviceProvider)
    {
        _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
        _baseUrl = "https://localhost:7101/api/Tasks";
    }

    /// <summary>
    /// Получает существующую задачу асинхронно
    /// </summary>
    /// <param name="request">Запрос на получение существующей задачи</param>
    /// <returns>Ответ существующей задачи.</returns>
    public async Task<ExistingTaskApiResponse> GetExistingTaskAsync(ExistingTaskApiRequest request)
    {
        var requestData = new HttpRequestData
        {
            Method = HttpMethod.Get,
            Uri = new Uri($"{_baseUrl}/{request.TaskId}")
        };

        var connectionData = new HttpConnectionData();
        var response = await _httpRequestService.SendRequestAsync<ExistingTaskApiResponse>(requestData, connectionData);

        if (response.IsSuccessStatusCode)
        {
            response.Body.IsExists = true;
            return response.Body;
        }
        else
            throw new Exception($"Request failed with status code {response.StatusCode}");
    }
}