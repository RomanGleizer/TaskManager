using Services.ViewModels.MemberViewModels;
using Services.ViewModels.RoleViewModels;

namespace Services.Interfaces;

/// <summary>
/// Сервис для работы с данными о участниками
/// </summary>
public interface IMemberService
{
    /// <summary>
    /// Получает роль по идентификатору участника
    /// </summary>
    /// <param name="id">Идентификатор участника</param>
    /// <returns>Модель представления участника или null, если участник не найден</returns>
    Task<MemberViewModel?> GetById(string id);
}