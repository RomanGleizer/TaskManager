﻿using Core.Dal.Base;
using TasksMicroservice.TasksMicroserviceDal.Entities;
using TasksMicroservice.TasksMicroserviceDal.EntityFramework;
using TasksMicroservice.TasksMicroserviceDal.Interfaces;

namespace TasksMicroservice.TasksMicroserviceDal.Repositories;

/// <summary>
///     Реализация интерфейса IUnitOfWork для работы с базой данных с использованием EF
/// </summary>
/// <remarks>
///     Инициализирует новый экземпляр класса EFUnitOfWork с заданным контекстом базы данных
/// </remarks>
public class EntityFrameworkUnitOfWork(TasksMicroserviceDbContext dbContext) : IUnitOfWork
{
    private IRepository<TaskDal, Guid>? _taskRepository;

    /// <summary>
    ///     Получает репозиторий для работы с сущностями TaskDal
    /// </summary>
    public IRepository<TaskDal, Guid> Tasks => _taskRepository ??= new TaskRepository(dbContext);
}