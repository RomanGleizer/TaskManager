using System.Threading.Tasks;
using Logic.DTO;

namespace Logic.Interfaces;

/// <summary>
/// Менеджер DTO проекта
/// </summary>
public interface IProjectDtoManager
{
    /// <summary>
    /// Асинхронно добавляет участника в проект
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="participantId">Идентификатор участника</param>
    Task<ProjectDTO> AddParticipantAsync(int projectId, string participantId);
}
