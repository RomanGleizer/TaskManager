using ConnectionLib.ConnectionServices.DtoModels.AddMemberInProject;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Core.RPC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConnectionLib.ConnectionServices;

/// <summary>
/// Сервис для управления соединением пользователей
/// </summary>
public class UserConnectionService : IUserConnectionService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpRequestService _httpRequestService;
    private readonly RPSConsumer _rpcConsumer;
    private readonly string _baseUrl = "https://localhost:7265/api/Users";

    public UserConnectionService(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _configuration = configuration;

        if (_configuration.GetSection("ConnectionType").Value == "http")
        {
            _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
        }
        else if (_configuration.GetSection("ConnectionType").Value == "rpc")
        {
            _rpcConsumer = new RPSConsumer();
        }
        else
        {
            throw new InvalidOperationException("Invalid configuration value for 'ConnectionType'");
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
            return await AddProjectWithRPC(request);
        }
        else
        {
            throw new InvalidOperationException("No valid communication method configured.");
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
        var addMemberResponse = await _httpRequestService.SendRequestAsync<AddProjectToListOfUserProjectsResponse>(addMemberInProjectData, connectionData);

        if (addMemberResponse.IsSuccessStatusCode)
            return addMemberResponse.Body;

        throw new HttpRequestException("Не удалось добавить участника в проект. Код состояния: " + addMemberResponse.StatusCode);
    }

    private async Task<AddProjectToListOfUserProjectsResponse> AddProjectWithRPC(AddProjectToListOfUserProjectsRequest request)
    {
        string message = $"JoinInProject {request.MemberId} {request.ProjectId}";
        string response = await _rpcConsumer.CallAsync(message);

        /// Ожидаем, что ответ имеет формат "MemberId ProjectId"
        var parts = response.Split(' ');
        if (parts.Length != 2)
        {
            throw new InvalidOperationException("Некорректный формат ответа от RPC.");
        }

        if (!Guid.TryParse(parts[0], out var memberId))
        {
            throw new InvalidOperationException("Невозможно распознать MemberId в ответе от RPC.");
        }

        if (!int.TryParse(parts[1], out var projectId))
        {
            throw new InvalidOperationException("Невозможно распознать ProjectId в ответе от RPC.");
        }

        return new AddProjectToListOfUserProjectsResponse
        {
            MemberId = memberId,
            ProjectId = projectId
        };
    }
}
