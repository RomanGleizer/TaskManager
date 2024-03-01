using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

/// <summary>
/// Репозиторий для работы с ролями
/// </summary>
public class RoleRepository : IRepository<RoleDal, int>
{
    private readonly TaskManagerDbContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр класса RoleRepository
    /// </summary>
    public RoleRepository(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получает все роли
    /// </summary>
    public async Task<IList<RoleDal>> GetAllAsync()
    {
        return await _dbContext.ProjectRoles.ToListAsync();
    }

    /// <summary>
    /// Асинхронно получает роль по ее идентификатору
    /// </summary>
    /// <param name="id">Идентификатор роли</param>
    public async Task<RoleDal?> GetByIdAsync(int id)
    {
        return await _dbContext.ProjectRoles.FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Асинхронно создает новую роль
    /// </summary>
    /// <param name="item">Новая роль для создания.</param>
    public async Task CreateAsync(RoleDal item)
    {
        await _dbContext.ProjectRoles.AddAsync(item);
    }

    /// <summary>
    /// Удаляет роль
    /// </summary>
    /// <param name="item">Роль для удаления</param>
    public async Task DeleteAsync(RoleDal item)
    {
        _dbContext.ProjectRoles.Remove(item);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Обновляет информацию о роли
    /// </summary>
    /// <param name="item">Роль для обновления</param>
    public async Task UpdateAsync(RoleDal item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
}
