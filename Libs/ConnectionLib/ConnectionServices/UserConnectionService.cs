using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
using ConnectionLib.ConnectionServices.DtoModels.AddProjectToListOfUserProjects;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Core.RPC;
using Logic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace ConnectionLib.ConnectionServices;

/// <summary>
/// Сервис для управления соединением пользователей
/// </summary>
public abstract class UserConnectionService<TService> : IUserConnectionService
    where TService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserConnectionService<TService>> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ObjectPool<IConnection> _connectionPool;

    private readonly string? _baseUrl;
    private readonly IHttpRequestService? _httpRequestService;
    private readonly RabbitMqBackgroundAddProjectService? _rpcConsumer;

    public UserConnectionService(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<UserConnectionService<TService>> logger,
        ObjectPool<IConnection> connectionPool)
    {
        _configuration = configuration;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _connectionPool = connectionPool;

        _baseUrl = configuration.GetValue<string>("BaseUrl:Users");

        if (_configuration.GetValue<string>("ConnectionType") == "http")
        {
            _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
        }
        else if (_configuration.GetValue<string>("ConnectionType") == "rabbitmq")
        {
            _rpcConsumer = new RabbitMqBackgroundAddProjectService(_serviceProvider, _connectionPool);
        }
        else
        {
            throw new InvalidOperationException("Недопустимое значение конфигурации для 'ConnectionType'");
        }
    }

    /// <inheritdoc/>
    public async Task<AddProjectToListOfUserProjectsResponse> AddProjectIdToListOfUserProjectIds(
        AddProjectToListOfUserProjectsRequest request)
    {
        if (_httpRequestService != null)
        {
            return await AddProjectWithHttp(request);
        }
        else if (_rpcConsumer != null)
        {
            await UserConnectionService<TService>.AddProjectWithRpc(request, "UserConnectionServiceQueue");
            return new AddProjectToListOfUserProjectsResponse
            {
                MemberId = request.MemberId,
                ProjectId = request.ProjectId
            };
        }
        else
        {
            throw new Exception("Не получилось настроить метод связи");
        }
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
            var addMemberResponse = await _httpRequestService.SendRequestAsync<AddProjectToListOfUserProjectsResponse>(addMemberInProjectData, connectionData);

            if (addMemberResponse.IsSuccessStatusCode)
                return addMemberResponse.Body;

            _logger.LogError("Не удалось добавить участника в проект. Код состояния: {StatusCode}", addMemberResponse.StatusCode);
            throw new HttpRequestException("Не удалось добавить участника в проект. Код состояния: " + addMemberResponse.StatusCode);
        }
        else
        {
            throw new Exception($"{typeof(IHttpRequestService)} имеет значение null");
        }
    }

    private static async Task AddProjectWithRpc(AddProjectToListOfUserProjectsRequest request, string queueName)
    {
        var publisher = new RPCPublisher<AddProjectToListOfUserProjectsRequest>(queueName, request);
        await publisher.PublishAsync();
        publisher.Dispose();
    }
}
