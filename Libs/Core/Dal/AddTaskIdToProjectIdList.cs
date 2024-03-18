using Core.Dal.Base;
using Domain.Interfaces;

namespace Core.Dal;

public class AddTaskIdToProjectIdList(IProjectRepository repository) : IAddTaskIdToProjectIdList
{
    private readonly IProjectRepository _repository = repository;

    public async Task AddNewTaskIdInProjectIdList(int projectId, int taskId)
    {
        var existingProject = await _repository.GetProjectByIdAsync(projectId);

        if (existingProject is ITaskIdsContainer container && !container.TaskIds.Contains(taskId))
        {
            container.TaskIds.Add(taskId);
            await _repository.SaveChangesAsync();
        }
    }
}
