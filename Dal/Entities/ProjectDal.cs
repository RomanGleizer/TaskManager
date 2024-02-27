﻿using Core.Dal.Base;

namespace Dal.Entities;

/// <summary>
/// Сущность проекта для базы данных
/// </summary>
public class ProjectDal : IBaseEntityDal<int>
{
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Имя проекта
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание проекта
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Последний момент обновления проекта
    /// </summary>
    public DateTime LastModifidedDate { get; set; }

    /// <summary>
    /// Основатель проекта
    /// </summary>
    public UserDal Creator { get; set; }

    /// <summary>
    /// Коллекция участников проекта
    /// </summary>
    public IList<UserDal> Participants { get; set; }

    /// <summary>
    /// Список задач в проекте
    /// </summary>
    public IList<TaskDal> Tasks { get; set; }
}
