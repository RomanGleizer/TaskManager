using ConnectionLibrary.ConnectionServices.BackgroundConnectionServices;
using ConnectionLibrary.ConnectionServices.DtoModels.AddProjectToListOfUserProjects;
using ConnectionLibrary.ConnectionServices.Interfaces;
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
///     Сервис для управления соединением пользователей
/// </summary>
public class UserConnectionService : IUserConnectionService
{
    private readonly string? _baseUrl;
    private readonly IConfiguration _configuration;
    private readonly ObjectPool<IConnection> _connectionPool;
    private readonly IHttpRequestService? _httpRequestService;
    private readonly ILogger<UserConnectionService> _logger;
    private readonly RabbitMqBackgroundAddProjectService? _rpcConsumer;
    private readonly IServiceProvider _serviceProvider;

    public UserConnectionService(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<UserConnectionService> logger,
        ObjectPool<IConnection> connectionPool)
    {
        _configuration = configuration;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _connectionPool = connectionPool;

        _baseUrl = configuration.GetValue<string>("BaseUrl:Users");

        if (_configuration.GetValue<string>("ConnectionType") == "http")
            _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
        else if (_configuration.GetValue<string>("ConnectionType") == "rabbitmq")
            _rpcConsumer = new RabbitMqBackgroundAddProjectService(_serviceProvider, _connectionPool);
        else
            throw new InvalidOperationException("Недопустимое значение конфигурации для 'ConnectionType'");
    }

    /// <inheritdoc />
    public async Task<AddProjectToListOfUserProjectsResponse> AddProjectIdToListOfUserProjectIds(
        AddProjectToListOfUserProjectsRequest request)
    {
        if (_httpRequestService != null) return await AddProjectWithHttp(request);

        if (_rpcConsumer == null) throw new Exception("Не получилось настроить метод связи");

        await AddProjectWithRpc(request, "UserConnectionServiceQueue");
        return new AddProjectToListOfUserProjectsResponse
        {
            MemberId = request.MemberId,
            ProjectId = request.ProjectId
        };
    }

    private async Task<AddProjectToListOfUserProjectsResponse> AddProjectWithHttp(
        AddProjectToListOfUserProjectsRequest request)
    {
        var addMemberInProjectData = new HttpRequestData
        {
            Method = HttpMethod.Post,
            Uri = new Uri($"{_baseUrl}/{request.MemberId}/projects/{request.ProjectId}")
        };

        var connectionData = new HttpConnectionData();

        if (_httpRequestService != null)
        {
            var addMemberResponse = await _httpRequestService.SendRequestAsync<AddProjectToListOfUserProjectsResponse>(
                addMemberInProjectData,
                connectionData);

            if (addMemberResponse.IsSuccessStatusCode)
                return addMemberResponse.Body;

            _logger.LogError(
                "Не удалось добавить участника в проект. Код состояния: {StatusCode}", addMemberResponse.StatusCode);

            throw new HttpRequestException(
                "Не удалось добавить участника в проект. Код состояния: " + addMemberResponse.StatusCode);
        }

        throw new Exception($"{typeof(IHttpRequestService)} имеет значение null");
    }

    private static async Task AddProjectWithRpc(AddProjectToListOfUserProjectsRequest request, string queueName)
    {
        var publisher = new RpcPublisher<AddProjectToListOfUserProjectsRequest>(queueName, request);
        await publisher.PublishAsync();
        publisher.Dispose();
    }
}