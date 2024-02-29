using Dal.EF;
using Dal.Entities;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями
/// </summary>
public class UserRepository : IRepository<UserDal, string>
{
    private readonly TaskManagerDbContext _dbContext;

    /// <summary>
    /// Конструктор репозитория пользователей
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    public UserRepository(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получает всех пользователей
    /// </summary>
    public IEnumerable<UserDal> GetAll()
    {
        return _dbContext.Users;
    }

    /// <summary>
    /// Получает пользователя по идентификатору асинхронно
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    public async Task<UserDal?> GetByIdAsync(string id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <summary>
    /// Создает пользователя асинхронно
    /// </summary>
    /// <param name="item">Пользователь для создания</param>
    public async Task CreateAsync(UserDal item)
    {
        await _dbContext.Users.AddAsync(item);
    }

    /// <summary>
    /// Удаляет пользователя
    /// </summary>
    /// <param name="item">Пользователь для удаления</param>
    public void Delete(UserDal item)
    {
        _dbContext.Users.Remove(item);
    }

    /// <summary>
    /// Обновляет пользователя
    /// </summary>
    /// <param name="item">Пользователь для обновления</param>
    public void Update(UserDal item)
    {
        _dbContext.Entry(item).State = EntityState.Modified;
    }
}
