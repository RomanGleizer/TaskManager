using ConnectionLib.ConnectionServices.DtoModels.TaskById;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Logic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConnectionLib.ConnectionServices;

/// <summary>
/// Сервис для управления соединением с задачами
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса TaskConnectionService с указанным провайдером сервисов
/// </remarks>
/// <param name="serviceProvider">Провайдер сервисов</param>
public class TaskConnectionService(
    IConfiguration configuration,
    IServiceProvider serviceProvider,
    ILogger<UserConnectionService<UserService>> logger) : ITaskConnectionService
{
    private readonly IHttpRequestService _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
    private readonly string? _baseUrl = configuration.GetValue<string>("BaseUrl:Tasks");

    /// <inheritdoc/>
    public async Task<ExistingTaskInProjectResponse> GetExistingTaskAsync(ExistingTaskInProjectRequest request)
    {
        var requestData = new HttpRequestData
        {
            Method = HttpMethod.Get,
            Uri = new Uri($"{_baseUrl}/projects/{request.ProjectId}")
        };

        var connectionData = new HttpConnectionData();
        var response = await _httpRequestService.SendRequestAsync<IList<Guid>>(requestData, connectionData).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            return new ExistingTaskInProjectResponse { TaskIds = response.Body };
        }
        else
        {
            logger.LogError("Не удалось найти задачу. Код состояния: {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"Запрос завершился неудачно с кодом состояния {response.StatusCode}");
        }
    }
}