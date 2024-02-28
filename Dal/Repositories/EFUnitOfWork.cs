﻿using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;

namespace Dal.Repositories;

/// <summary>
/// Реализация интерфейса IUnitOfWork для работы с базой данных с использованием EF.
/// </summary>
public class EFUnitOfWork : IUnitOfWork
{
    private readonly TaskManagerDbContext _dbContext;
    private IRepository<TaskDal, int>? _taskRepository;
    private IRepository<ProjectDal, int>? _projectRepository;

    /// <summary>
    /// Инициализирует новый экземпляр класса EFUnitOfWork с заданным контекстом базы данных.
    /// </summary>
    public EFUnitOfWork(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получает репозиторий для работы с сущностями ProjectTask.
    /// </summary>
    public IRepository<TaskDal, int> Tasks
    {
        get
        {
            if (_taskRepository == null)
                _taskRepository = new TaskRepository(_dbContext);
            return _taskRepository;
        }
    }

    public IRepository<ProjectDal, int> Projects
    {
        get
        {
            if (_projectRepository == null)
                _projectRepository = new ProjectRepository(_dbContext);
            return _projectRepository;
        }
    }

    /// <summary>
    /// Асинхронно сохраняет все изменения, сделанные в базе данных.
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
