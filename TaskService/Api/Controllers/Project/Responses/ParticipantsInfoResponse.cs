using Dal.Entities;
using Logic.DTO;

namespace Api.Controllers.Project.Responses;

/// <summary>
/// Представляет модель ответа на запрос получения всех участников проекта
/// </summary>
public class ParticipantsInfoResponse
{
    /// <summary>
    /// Список участников проекта
    /// </summary>
    public required IList<UserDTO> Participants { get; init; }
}
