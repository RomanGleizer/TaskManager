using ConnectionLibrary.ConnectionServices.BackgroundConnectionServices;
using ConnectionLibrary.ConnectionServices.DtoModels.AddTaskInProject;
using ConnectionLibrary.ConnectionServices.DtoModels.ProjectById;
using ConnectionLibrary.ConnectionServices.Interfaces;
using Core.Dal.Base;
using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Core.Rpc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace ConnectionLibrary.ConnectionServices;

/// <summary>
///     Сервис для управления соединением с проектами
/// </summary>
/// <remarks>
///     Инициализирует новый экземпляр класса ProjectConnectionService с указанным провайдером сервисов
/// </remarks>
public class ProjectConnectionService<TModel> : IProjectConnectionService
    where TModel : IBaseEntity<Guid>
{
    private readonly RabbitMqBackgroundAddTaskService? _addTaskRpcConsumer;
    private readonly string? _baseUrl;
    private readonly IConfiguration _configuration;
    private readonly RabbitMqBackgroundGetProjectService<TModel>? _getProjectRpcConsumer;

    private readonly IHttpRequestService? _httpRequestService;
    private readonly ILogger<ProjectConnectionService<TModel>> _logger;
    private readonly IServiceProvider _serviceProvider;

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
            _addTaskRpcConsumer = new RabbitMqBackgroundAddTaskService(_serviceProvider, connectionPool);
            _getProjectRpcConsumer = new RabbitMqBackgroundGetProjectService<TModel>(_serviceProvider, connectionPool);
        }
        else
        {
            throw new InvalidOperationException("Недопустимое значение конфигурации для 'ConnectionType'");
        }
    }

    /// <inheritdoc />
    public async Task<IsProjectExistsResponse> GetProjectByIdAsync(IsProjectExistsRequest request)
    {
        if (_httpRequestService != null)
            return await GetProjectByIdWithHttp(request);

        if (_getProjectRpcConsumer == null) throw new Exception("Не получилось настроить метод связи");
        var isMessagePublished = await GetProjectByIdWithRpc(request, "GetProjectQueue");

        if (isMessagePublished)
            return new IsProjectExistsResponse
            {
                Id = request.ProjectId,
                Name = default,
                CreationDate = default,
                TaskIds = []
            };

        throw new Exception("Сообщение не было доставлено");
    }

    /// <inheritdoc />
    public async Task<AddTaskIdInProjectTaskIdsResponse> AddTaskIdInProjectTaskIdsAsync(
        AddTaskIdInProjectTaskIdsRequest request)
    {
        if (_httpRequestService != null)
            return await AddTaskIdInProjectTaskIdsWithHttp(request);

        if (_addTaskRpcConsumer == null) throw new Exception("Не получилось настроить метод связи");
        var isMessagePublished = await AddTaskIdInProjectTaskIdsWithRpc(request, "AddTaskQueue");

        if (isMessagePublished)
            return new AddTaskIdInProjectTaskIdsResponse
            {
                ProjectId = request.ProjectId,
                TaskIds = []
            };

        throw new Exception("Сообщение не было доставлено");
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
            var projectIdResponse = await _httpRequestService.SendRequestAsync<IsProjectExistsResponse>(
                getIdRequestData,
                connectionData);

            if (projectIdResponse.IsSuccessStatusCode)
                return projectIdResponse.Body;

            throw new HttpRequestException(
                "Проект не найден в базе данных. Код состояния: " + projectIdResponse.StatusCode);
        }

        throw new Exception($"{typeof(IHttpRequestService)} имеет значение null");
    }

    private static async Task<bool> GetProjectByIdWithRpc(IsProjectExistsRequest request, string queueName)
    {
        var publisher = new RpcPublisher<IsProjectExistsRequest>(queueName, request);
        var result = await publisher.PublishAsync();
        publisher.Dispose();

        return result;
    }

    private async Task<AddTaskIdInProjectTaskIdsResponse> AddTaskIdInProjectTaskIdsWithHttp(
        AddTaskIdInProjectTaskIdsRequest request)
    {
        var addTaskInProjectData = new HttpRequestData
        {
            Method = HttpMethod.Post,
            Uri = new Uri($"{_baseUrl}/{request.ProjectId}/tasks/{request.TaskId}")
        };

        var connectionData = new HttpConnectionData();

        if (_httpRequestService != null)
        {
            var addTaskResponse = await _httpRequestService.SendRequestAsync<AddTaskIdInProjectTaskIdsResponse>(
                addTaskInProjectData,
                connectionData);

            if (addTaskResponse.IsSuccessStatusCode)
                return addTaskResponse.Body;

            _logger.LogError(
                $"Не удалось добавить задачу в проект. Код состояния: {addTaskResponse.StatusCode}");

            throw new HttpRequestException(
                $"Не удалось добавить участника в проект. Код состояния: {addTaskResponse.StatusCode}");
        }

        throw new Exception($"{typeof(IHttpRequestService)} имеет значение null");
    }

    private static async Task<bool> AddTaskIdInProjectTaskIdsWithRpc(
        AddTaskIdInProjectTaskIdsRequest request,
        string queueName)
    {
        var publisher = new RpcPublisher<AddTaskIdInProjectTaskIdsRequest>(queueName, request);
        var result = await publisher.PublishAsync();
        publisher.Dispose();

        return result;
    }
}