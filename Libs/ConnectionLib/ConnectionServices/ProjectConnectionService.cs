using ConnectionLib.ConnectionServices.BackgroundConnectionService;
using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
using ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;
using ConnectionLib.ConnectionServices.DtoModels.ProjectById;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.Dal.Base;
using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Core.RPC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace ConnectionLib.ConnectionServices;

/// <summary>
/// Сервис для управления соединением с проектами
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса ProjectConnectionService с указанным провайдером сервисов
/// </remarks>
/// <param name="serviceProvider">Провайдер сервисов</param>
public class ProjectConnectionService<TModel> : IProjectConnectionService
    where TModel : IBaseEntity<int>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ProjectConnectionService<TModel>> _logger;
    private readonly IServiceProvider _serviceProvider;

    private readonly IHttpRequestService? _httpRequestService;
    private readonly RabbitMQBackgroundAddTaskService? _addTaskRpcConsumer;
    private readonly RabbitMQBackgroundGetProjectService<TModel>? _getProjectRpcConsumer;
    private readonly string? _baseUrl;

    public ProjectConnectionService(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<ProjectConnectionService<TModel>> logger,
        ObjectPool<IConnection> connectionPool)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _logger = logger;

        _baseUrl = configuration.GetValue<string>("BaseUrl:Projects");

        if (_configuration.GetValue<string>("ConnectionType") == "http")
        {
            _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
        }
        else if (_configuration.GetValue<string>("ConnectionType") == "rabbitmq")
        {
            _addTaskRpcConsumer = new RabbitMQBackgroundAddTaskService(_serviceProvider, connectionPool);
            _getProjectRpcConsumer = new RabbitMQBackgroundGetProjectService<TModel>(_serviceProvider, connectionPool);
        }
        else
        {
            throw new InvalidOperationException("Недопустимое значение конфигурации для 'ConnectionType'");
        }
    }

    /// <inheritdoc/>
    public async Task<IsProjectExistsResponse> GetProjectByIdAsync(IsProjectExistsRequest request)
    {
        if (_httpRequestService != null)
            return await GetProjectByIdWithHttp(request);

        if (_getProjectRpcConsumer != null)
        {
            var isMessagePublished = await ProjectConnectionService<TModel>.GetProjectByIdWithRpc(request, "GetProjectQueue");

            if (isMessagePublished)
            {
                return new IsProjectExistsResponse
                {
                    Id = request.ProjectId,
                    Name = default,
                    CreationDate = default,
                    TaskIds = []
                };
            }
            else
                throw new Exception("Сообщение не было доставлено");
        }

        throw new Exception("Не получилось настроить метод связи");
    }

    /// <inheritdoc/>
    public async Task<AddTaskIdInProjectTaskIdsResponse> AddTaskIdInProjectTaskIdsAsync(AddTaskIdInProjectTaskIdsRequest request)
    {
        if (_httpRequestService != null)
            return await AddTaskIdInProjectTaskIdsWithHttp(request);

        if (_addTaskRpcConsumer != null)
        {
            var isMessagePublished = await ProjectConnectionService<TModel>.AddTaskIdInProjectTaskIdsWithRPC(request, "AddTaskQueue");

            if (isMessagePublished)
            {
                return new AddTaskIdInProjectTaskIdsResponse
                {
                    ProjectId = request.ProjectId,
                    TaskIds = []
                };
            }
            else
                throw new Exception("Сообщение не было доставлено");
        }

        throw new Exception("Не получилось настроить метод связи");
    }

    private async Task<IsProjectExistsResponse> GetProjectByIdWithHttp(IsProjectExistsRequest request)
    {
        var getIdRequestData = new HttpRequestData
        {
            Method = HttpMethod.Get,
            Uri = new Uri($"{_baseUrl}/{request.ProjectId}")
        };

        var connectionData = new HttpConnectionData();

        if (_httpRequestService != null)
        {
            var projectIdResponse = await _httpRequestService.SendRequestAsync<IsProjectExistsResponse>(getIdRequestData, connectionData);

            if (projectIdResponse.IsSuccessStatusCode)
                return projectIdResponse.Body;

            throw new HttpRequestException("Проект не найден в базе данных. Код состояния: " + projectIdResponse.StatusCode);
        }
        else
        {
            throw new Exception($"{typeof(IHttpRequestService)} имеет значение null");
        }
    }

    private static async Task<bool> GetProjectByIdWithRpc(IsProjectExistsRequest request, string queueName)
    {
        var publisher = new RPCPublisher<IsProjectExistsRequest>(queueName, request);
        var result = await publisher.PublishAsync();
        publisher.Dispose();

        return result;
    }

    private async Task<AddTaskIdInProjectTaskIdsResponse> AddTaskIdInProjectTaskIdsWithHttp(AddTaskIdInProjectTaskIdsRequest request)
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
            throw new Exception($"{typeof(IHttpRequestService)} имеет значение null");
        }
    }

    private static async Task<bool> AddTaskIdInProjectTaskIdsWithRPC(AddTaskIdInProjectTaskIdsRequest request, string queueName)
    {
        var publisher = new RPCPublisher<AddTaskIdInProjectTaskIdsRequest>(queueName, request);
        var result = await publisher.PublishAsync();
        publisher.Dispose();

        return result;
    }
}