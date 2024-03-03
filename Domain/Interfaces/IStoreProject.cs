using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreProject
{
    Task<int> AddPost(Project project);
}
