using Dal.Entities;

namespace Api.Controllers.Project.Requests;

/// <summary>
/// Модель запроса на добавление нового участника в проект
/// </summary>
public class AddParticipantRequest
{
    /// <summary>
    /// Получает или устанавливает информацию о новом участнике проекта
    /// </summary>
    public required UserDal NewParticipant { get; init; }
}
