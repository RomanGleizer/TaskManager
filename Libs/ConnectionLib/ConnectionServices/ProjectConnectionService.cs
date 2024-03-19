using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
using ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;
using ConnectionLib.ConnectionServices.DtoModels.ProjectById;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Core.RPC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConnectionLib.ConnectionServices;

/// <summary>
/// Сервис для управления соединением с проектами
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса ProjectConnectionService с указанным провайдером сервисов
/// </remarks>
/// <param name="serviceProvider">Провайдер сервисов</param>
public class ProjectConnectionService : IProjectConnectionService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ProjectConnectionService> _logger;
    private readonly IServiceProvider _serviceProvider;

    private readonly IHttpRequestService? _httpRequestService;
    private readonly RabbitMQBackgroundUserConnectionService? _rpcConsumer;
    private readonly string _baseUrl;

    public ProjectConnectionService(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<ProjectConnectionService> logger)
    {
        _baseUrl = "https://localhost:7047/api/Projects";
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();

        if (_configuration.GetSection("ConnectionType").Value == "rpc")
        {
            _rpcConsumer = new RabbitMQBackgroundUserConnectionService(_serviceProvider);
        }
        else
        {
            throw new InvalidOperationException("Недопустимое значение конфигурации для 'ConnectionType'");
        }
    }

    /// <inheritdoc/>
    public async Task<ExistingProjectApiResponse> GetProjectByIdAsync(ExistingProjectApiRequest request)
    {
        var getIdRequestData = new HttpRequestData
        {
            Method = HttpMethod.Get,
            Uri = new Uri($"{_baseUrl}/{request.ProjectId}")
        };

        var connectionData = new HttpConnectionData();

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
        var projectIdResponse = await _httpRequestService.SendRequestAsync<ExistingProjectApiResponse>(getIdRequestData, connectionData);
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

        if (projectIdResponse.IsSuccessStatusCode)
            return projectIdResponse.Body;

        throw new HttpRequestException("Проект не найден в базе данных. Код состояния: " + projectIdResponse.StatusCode);
    }

    /// <inheritdoc/>
    public async Task<AddTaskIdInProjectTaskIdsResponse> AddTaskIdInProjectTaskIdsAsync(AddTaskIdInProjectTaskIdsRequest request)
    {
        //if (_httpRequestService != null)
        //{
        //    return await AddTaskIdInProjectTaskIdsWithHttp(request);
        //}

        if (_rpcConsumer != null)
        {
            await AddTaskIdInProjectTaskIdsWithRPC(request, "ProjectConnectionServiceQueue");
            return new AddTaskIdInProjectTaskIdsResponse
            {
                ProjectId = request.ProjectId,
                TaskIds = []
            };
        }
        else
        {
            throw new Exception("Не получилось настроить метод связи");
        }
    }

    public async Task<AddTaskIdInProjectTaskIdsResponse> AddTaskIdInProjectTaskIdsWithHttp(AddTaskIdInProjectTaskIdsRequest request)
    {
        var addTaskInProjectData = new HttpRequestData
        {
            Method = HttpMethod.Post,
            Uri = new Uri($"{_baseUrl}/{request.ProjectId}/tasks/{request.TaskId}")
        };

        var connectionData = new HttpConnectionData();

        if (_httpRequestService != null)
        {
            var addTaskResponse = await _httpRequestService.SendRequestAsync<AddTaskIdInProjectTaskIdsResponse>(addTaskInProjectData, connectionData);

            if (addTaskResponse.IsSuccessStatusCode)
                return addTaskResponse.Body;

            _logger.LogError($"Не удалось добавить задачу в проект. Код состояния: {addTaskResponse.StatusCode}");
            throw new HttpRequestException($"Не удалось добавить участника в проект. Код состояния: {addTaskResponse.StatusCode}");
        }
        else
        {
            throw new Exception($"{typeof(ProjectConnectionService)} имеет значение null");
        }
    }

    private async Task AddTaskIdInProjectTaskIdsWithRPC(AddTaskIdInProjectTaskIdsRequest request, string queueName)
    {
        var publisher = new RPCPublisher<AddTaskIdInProjectTaskIdsRequest>(queueName, request);
        await publisher.PublishAsync();
        publisher.Dispose();
    }
}