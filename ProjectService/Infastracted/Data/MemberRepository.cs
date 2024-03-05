using Domain.Entities;
using Domain.Interfaces;
using Infastracted.EF;
using Microsoft.EntityFrameworkCore;

namespace Infastracted.Data;

/// <summary>
/// Реализация интерфейса репозитория для работы с данными участников проекта
/// </summary>
public class MemberRepository : IMemberRepository
{
    private readonly ProjectServiceDbContext _dbContext;

    public MemberRepository(ProjectServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получает участника по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор участника</param>
    /// <returns>Объект члена или null, если участник не найден</returns>
    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        return await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == id);
    }
}