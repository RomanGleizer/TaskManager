﻿using ConnectionLibrary.ConnectionServices.DtoModels.ExistingTaskInProject;

namespace ConnectionLibrary.ConnectionServices.Interfaces;

/// <summary>
///     Интерфейс сервиса подключения к задачам
/// </summary>
public interface ITaskConnectionService
{
    /// <summary>
    ///     Асинхронно запрашивает информацию о существующей задаче
    /// </summary>
    /// <param name="request">Запрос на получение информации о задаче</param>
    /// <returns>Задача, представляющая операцию получения информации о задаче</returns>
    Task<ExistingTaskInProjectResponse> GetExistingTaskAsync(ExistingTaskInProjectRequest request);
}