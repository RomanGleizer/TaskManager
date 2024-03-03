﻿using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;

namespace Dal.Repositories;

/// <summary>
/// Реализация интерфейса IUnitOfWork для работы с базой данных с использованием EF
/// </summary>
public class EFUnitOfWork : IUnitOfWork
{
    private readonly TaskManagerDbContext _dbContext;
    private IRepository<TaskDal, int>? _taskRepository;
    private IRepository<ProjectDal, int>? _projectRepository;
    private IRepository<CommentDal, int>? _commentRepository;
    private IRepository<UserDal, string>? _userRepository;
    private IRepository<RoleDal, int>? _roleRepository;

    /// <summary>
    /// Инициализирует новый экземпляр класса EFUnitOfWork с заданным контекстом базы данных
    /// </summary>
    public EFUnitOfWork(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получает репозиторий для работы с сущностями TaskDal
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

    /// <summary>
    /// Получает репозиторий для работы с сущностями ProjectDal
    /// </summary>
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
    /// Получает репозиторий для работы с сущностями CommentDal
    /// </summary>
    public IRepository<CommentDal, int> Comments
    {
        get
        {
            if (_commentRepository == null)
                _commentRepository = new CommentRepository(_dbContext);
            return _commentRepository;
        }
    }

    /// <summary>
    /// Получает репозиторий для работы с сущностями CommentDal
    /// </summary>
    public IRepository<UserDal, string> Users
    {
        get
        {
            if (_userRepository == null)
                _userRepository = new UserRepository(_dbContext);
            return _userRepository;
        }
    }

    /// <summary>
    /// Получает репозиторий для работы с сущностями RoleDal
    /// </summary>
    public IRepository<RoleDal, int> Roles
    {
        get
        {
            if (_roleRepository == null)
                _roleRepository = new RoleRepository(_dbContext);
            return _roleRepository;
        }
    }
}