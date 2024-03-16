using ConnectionLib.ConnectionServices.BackgroundConnectionServices;
using ConnectionLib.ConnectionServices.DtoModels.AddProjectToListOfUserProjects;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Core.RPC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConnectionLib.ConnectionServices;

/// <summary>
/// Сервис для управления соединением пользователей
/// </summary>
public class UserConnectionService : IUserConnectionService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserConnectionService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHttpRequestService? _httpRequestService;
    private readonly RabbitMQBackgroundUserConnectionService? _rpcConsumer;
    private readonly string _baseUrl = "https://localhost:7265/api/Users";

    public UserConnectionService(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<UserConnectionService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _configuration = configuration;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;

        if (_configuration.GetSection("ConnectionType").Value == "http")
        {
            _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
        }
        else if (_configuration.GetSection("ConnectionType").Value == "rpc")
        {
            _rpcConsumer = new RabbitMQBackgroundUserConnectionService(_serviceScopeFactory);
        }
        else
        {
            throw new InvalidOperationException("Недопустимое значение конфигурации для 'ConnectionType'");
        }
    }

    /// <inheritdoc/>
    public async Task<AddProjectToListOfUserProjectsResponse> AddProjectToListOfUserProjects(AddProjectToListOfUserProjectsRequest request)
    {
        if (_httpRequestService != null)
        {
            return await AddProjectWithHttp(request);
        }
        else if (_rpcConsumer != null)
        {
            await AddProjectWithRPC(request, "UserConnectionServiceQueue");
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

    private async Task<AddProjectToListOfUserProjectsResponse> AddProjectWithHttp(AddProjectToListOfUserProjectsRequest request)
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
            throw new Exception("UserConnectionService имеет значение null");
        }
    }

    private async Task AddProjectWithRPC(AddProjectToListOfUserProjectsRequest request, string queueName)
    {
        var publisher = new RPCPublisher<AddProjectToListOfUserProjectsRequest>(queueName, request);
        await publisher.PublishAsync();
        publisher.Dispose();
    }
}
