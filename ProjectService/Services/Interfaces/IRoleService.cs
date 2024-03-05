using Services.ViewModels.MemberViewModels;

namespace Services.Interfaces;

/// <summary>
/// Сервис для работы с ролями
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Получает информацию о роли по её идентификатору
    /// </summary>
    /// <param name="id">Идентификатор роли</param>
    /// <returns>Модель представления роли или null, если роль не найдена</returns>
    Task<MemberViewModel?> GetById(int id);
}