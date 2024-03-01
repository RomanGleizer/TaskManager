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
    public async Task<IList<ProjectDal>> GetAllAsync()
    {
        return await _dbContext.Projects.ToListAsync();
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
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Удаляет проект
    /// </summary>
    /// <param name="item">Проект для удаления</param>
    public async Task DeleteAsync(ProjectDal item)
    {
        _dbContext.Projects.Remove(item);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Обновляет проект
    /// </summary>
    /// <param name="item">Проект для обновления</param>
    public async Task UpdateAsync(ProjectDal item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
}
