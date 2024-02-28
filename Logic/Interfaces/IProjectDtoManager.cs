using Dal.Entities;
using Logic.DTO;

namespace Logic.Interfaces;

public interface IProjectDtoManager
{
    Task<ProjectDTO> AddParticipantAsync(int projectId, string participantId);
}
