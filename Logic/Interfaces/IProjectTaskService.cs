﻿using Logic.DTO;

namespace Logic.Interfaces;

/// <summary>
/// Сервис задач, предназначенный для операций над задачами
/// </summary>
public interface IProjectTaskService
{
    /// <summary>
    /// Получает задачу по идентификатору асинхронно.
    /// </summary>
    Task<TaskDTO> GetTaskByIdAsync(int id);

    /// <summary>
    /// Получает все задачи.
    /// </summary>
    IEnumerable<TaskDTO> GetAllTasks();

    /// <summary>
    /// Создает новую задачу асинхронно.
    /// </summary>
    Task CreateTaskAsync(TaskDTO task);

    /// <summary>
    /// Удаляет задачу по идентификатору асинхронно.
    /// </summary>
    Task DeleteTaskAsync(int id);

    /// <summary>
    /// Обновляет задачу по идентификатору асинхронно.
    /// </summary>
    Task UpdateTaskAsync(TaskDTO task, int id);
}
