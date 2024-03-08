using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ConnectionLib.ConnectionServices.DtoModels.AddTaskInProject;
using ConnectionLib.ConnectionServices.Interfaces;
using ConnectionLib.ConnectionServices.DtoModels.ProjectById;

namespace ConnectionLib.ConnectionServices;

/// <summary>
/// Сервис для управления соединением с проектами
/// </summary>
public class ProjectConnectionService : IProjectConnectionService
{
    private readonly IHttpRequestService _httpRequestService;
    private readonly string _baseUrl;

    /// <summary>
    /// Инициализирует новый экземпляр класса ProjectConnectionService с указанным провайдером сервисов
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов</param>
    public ProjectConnectionService(IServiceProvider serviceProvider)
    {
        _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
        _baseUrl = "https://localhost:7047/api/Projects";
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
        var projectIdResponse = await _httpRequestService.SendRequestAsync<ExistingProjectApiResponse>(getIdRequestData, connectionData).ConfigureAwait(false);

        if (projectIdResponse.IsSuccessStatusCode)
            return projectIdResponse.Body;

        throw new HttpRequestException("Проект не найден в базе данных. Код состояния: " + projectIdResponse.StatusCode);
    }

    /// <inheritdoc/>
    public async Task<AddTaskInProjectApiResponse> AddTaskInProjectAsync(AddTaskInProjectApiRequest request)
    {
        var addTaskInProjectData = new HttpRequestData
        {
            Method = HttpMethod.Post,
            Uri = new Uri($"{_baseUrl}/{request.ProjectId}/tasks/{request.TaskId}")
        };

        var connectionData = new HttpConnectionData();
        var addTaskResponse = await _httpRequestService.SendRequestAsync<AddTaskInProjectApiResponse>(addTaskInProjectData, connectionData).ConfigureAwait(false);

        if (addTaskResponse.IsSuccessStatusCode)
            return addTaskResponse.Body;

        throw new HttpRequestException("Не удалось добавить задачу в проект. Код состояния: " + addTaskResponse.StatusCode);
    }
}