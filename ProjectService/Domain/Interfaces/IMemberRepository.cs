using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Интерфейс для работы с данными участников
/// </summary>
public interface IMemberRepository
{
    /// <summary>
    /// Получает участника по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор участника</param>
    /// <returns>Объект участника или null, если участник не найден</returns>
    Task<Member?> GetMemberByIdAsync(string id);
}
