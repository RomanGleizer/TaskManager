using Domain.Entities;

namespace Services.Interfaces.ProjectInterfaces;

public interface ICreateProject
{
    Task<int> CreateProjectAsync(Project project);
}
