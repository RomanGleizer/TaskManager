using Domain.Updated;

namespace Services.Interfaces.Project_Interfaces;

public interface IUpdateProject
{
    Task<int> UpdateProjectAsync(int projectId, UpdateProjectData project);
}
