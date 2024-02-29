using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

/// <summary>
/// Репозиторий для работы с проектами
/// </summary>
public class ProjectRepository : IRepository<ProjectDal, int>
{
    private readonly TaskManagerDbContext _dbContext;

    /// <summary>
    /// Конструктор репозитория проектов
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    public ProjectRepository(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получает все проекты
    /// </summary>
    public IEnumerable<ProjectDal> GetAll()
    {
        return _dbContext.Projects;
    }

    /// <summary>
    /// Получает проект по идентификатору асинхронно
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    public async Task<ProjectDal?> GetByIdAsync(int id)
    {
        return await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Создает проект асинхронно
    /// </summary>
    /// <param name="item">Проект для создания</param>
    public async Task CreateAsync(ProjectDal item)
    {
        await _dbContext.Projects.AddAsync(item);
    }

    /// <summary>
    /// Удаляет проект
    /// </summary>
    /// <param name="item">Проект для удаления</param>
    public void Delete(ProjectDal item)
    {
        _dbContext.Projects.Remove(item);
    }

    /// <summary>
    /// Обновляет проект
    /// </summary>
    /// <param name="item">Проект для обновления</param>
    public void Update(ProjectDal item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
    }
}
