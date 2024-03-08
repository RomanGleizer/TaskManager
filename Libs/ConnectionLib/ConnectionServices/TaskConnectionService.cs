using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ConnectionLib.ConnectionServices.DtoModels.TaskById;
using ConnectionLib.ConnectionServices.Interfaces;

namespace ConnectionLib.ConnectionServices;

/// <summary>
/// Сервис для управления соединением с задачами
/// </summary>
public class TaskConnectionService : ITaskConnectionService
{
    private readonly IHttpRequestService _httpRequestService;
    private readonly string _baseUrl;

    /// <summary>
    /// Инициализирует новый экземпляр класса TaskConnectionService с указанным провайдером сервисов
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов</param>
    public TaskConnectionService(IServiceProvider serviceProvider)
    {
        _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
        _baseUrl = "https://localhost:7101/api/Tasks";
    }

    /// <inheritdoc/>
    public async Task<ExistingTaskApiResponse> GetExistingTaskAsync(ExistingTaskApiRequest request)
    {
        var requestData = new HttpRequestData
        {
            Method = HttpMethod.Get,
            Uri = new Uri($"{_baseUrl}/{request.TaskId}")
        };

        var connectionData = new HttpConnectionData();
        var response = await _httpRequestService.SendRequestAsync<ExistingTaskApiResponse>(requestData, connectionData).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
            return new ExistingTaskApiResponse { IsExists = true };
        else
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
    }
}