using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
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
public class TaskConnectionService : ITaskConnectionService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserConnectionService<UserService>> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _baseUrl;

    private readonly RabbitMQBackgroundTaskConnectionService? _rpcConsumer;
    private readonly IHttpRequestService? _httpRequestService;

    public TaskConnectionService(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<UserConnectionService<UserService>> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _baseUrl = "https://localhost:7101/api/Tasks";
        _configuration = configuration;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;

        if (_configuration.GetSection("ConnectionType").Value == "http")
        {
            _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
        }
        else if (_configuration.GetSection("ConnectionType").Value == "rpc")
        {
            _rpcConsumer = new RabbitMQBackgroundTaskConnectionService(_serviceScopeFactory);
        }
        else
        {
            throw new InvalidOperationException("Недопустимое значение конфигурации для 'ConnectionType'");
        }
    }

    /// <inheritdoc/>
    public async Task<ExistingTaskInProjectResponse> GetExistingTaskAsync(ExistingTaskInProjectRequest request)
    {
        return new ExistingTaskInProjectResponse
        {
            TaskIds = default
        };
    }

    private async Task<ExistingTaskInProjectResponse> GetExistingTaskWithHttp(ExistingTaskInProjectRequest request)
    {
        var requestData = new HttpRequestData
        {
            Method = HttpMethod.Get,
            Uri = new Uri($"{_baseUrl}/projects/{request.ProjectId}")
        };

        var connectionData = new HttpConnectionData();
        var response = await _httpRequestService.SendRequestAsync<IList<int>>(requestData, connectionData).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            return new ExistingTaskInProjectResponse { TaskIds = response.Body };
        }
        else
        {
            _logger.LogError("Не удалось найти задачу. Код состояния: {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"Запрос завершился неудачно с кодом состояния {response.StatusCode}");
        }
    }

    private async Task GetExistingTaskWithRpc()
    {

    }
}