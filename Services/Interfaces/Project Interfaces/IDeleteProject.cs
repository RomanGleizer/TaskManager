using Domain.Entities;

namespace Services.Interfaces.Project_Interfaces;

public interface IDeleteProject
{
    Task<int> DeleteProjectAsync(int id);
}
