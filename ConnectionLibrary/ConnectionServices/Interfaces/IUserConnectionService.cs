﻿using ConnectionLibrary.ConnectionServices.DtoModels.AddProjectToListOfUserProjects;

namespace ConnectionLibrary.ConnectionServices.Interfaces;

/// <summary>
///     Интерфейс сервиса для управления соединением пользователей.
/// </summary>
public interface IUserConnectionService
{
    /// <summary>
    ///     Асинхронно добавляет нового участника в проект
    /// </summary>
    /// <param name="request">Запрос на добавление участника в проект</param>
    /// <returns>Задача, представляющая операцию добавления участника в проект</returns>
    Task<AddProjectToListOfUserProjectsResponse> AddProjectIdToListOfUserProjectIds(
        AddProjectToListOfUserProjectsRequest request);
}