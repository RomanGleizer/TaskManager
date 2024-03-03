using Domain.Entities;

namespace Services.Interfaces.ProjectInterfaces;

public interface IFindProject
{
    Task<Project?> GetProjectByIdAsync(int projectId);
}
