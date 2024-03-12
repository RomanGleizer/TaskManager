using ConnectionLib.ConnectionServices.DtoModels.AddMemberInProject;
using ConnectionLib.ConnectionServices.Interfaces;
using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ConnectionLib.ConnectionServices;

/// <summary>
/// Сервис для управления соединением пользователей
/// </summary>
public class UserConnectionService(IServiceProvider serviceProvider) : IUserConnectionService
{
    private readonly IHttpRequestService _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
    private readonly string _baseUrl = "https://localhost:7265/api/Users";

    /// <inheritdoc/>
    public async Task<AddProjectToListOfUserProjectsResponse> AddProjectToListOfUserProjects(AddProjectToListOfUserProjectsRequest request)
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
}
