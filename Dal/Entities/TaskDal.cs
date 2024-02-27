﻿using Core.Dal.Base;

namespace Dal.Entities;

/// <summary>
/// Сущность задачи для базы данных
/// </summary>
public class TaskDal : IBaseEntityDal<int>
{
    /// <summary>
    /// Идентификатор задачи
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название задачи
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание задачи
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Текущий статус выполнения
    /// </summary>
    public ExecutionStatus ExecutionStatus { get; set; }

    /// <summary>
    /// Дата создания задачи
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Дата последнего внесенного изменения
    /// </summary>
    public DateTime LastUpdateDate { get; set; }

    /// <summary>
    /// Идентификаторы исполнителей задачи
    /// </summary>
    public IList<string> PerformerIds { get; set; }

    /// <summary>
    /// Исполнители задачи
    /// </summary>
    public IList<UserDal> Performers { get; set; }

    /// <summary>
    /// Идентификатор проекта, к которому относится задача 
    /// </summary>
    public int? ProjectId { get; set; }

    /// <summary>
    /// Ссылка на проект, к которому относится задача
    /// </summary>
    public ProjectDal Project { get; set; }
}