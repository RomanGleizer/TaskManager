using ConnectionLibrary.ConnectionServices.DtoModels.AddTaskInProject;
using ConnectionLibrary.ConnectionServices.DtoModels.ProjectById;

namespace ConnectionLibrary.ConnectionServices.Interfaces;

/// <summary>
///     Интерфейс сервиса подключения к проекту
/// </summary>
public interface IProjectConnectionService
{
    /// <summary>
    ///     Асинхронно запрашивает информацию о существующем проекте по его идентификатору
    /// </summary>
    /// <param name="request">Запрос на получение информации о проекте</param>
    /// <returns>Задача, представляющая операцию получения информации о проекте</returns>
    Task<IsProjectExistsResponse> GetProjectByIdAsync(IsProjectExistsRequest request);

    /// <summary>
    ///     Асинхронно добавляет новую задачу в проект
    /// </summary>
    /// <param name="request">Запрос на добавление задачи в проект</param>
    /// <returns>Задача, представляющая операцию добавления задачи в проект</returns>
    Task<AddTaskIdInProjectTaskIdsResponse> AddTaskIdInProjectTaskIdsAsync(AddTaskIdInProjectTaskIdsRequest request);
}