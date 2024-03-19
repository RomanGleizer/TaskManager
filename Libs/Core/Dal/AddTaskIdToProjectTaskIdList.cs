using Core.Dal.Base;

namespace Core.Dal;

/// <summary>
/// Реализация контракта IAddTaskIdToProjectIdList
/// </summary>
/// <param name="repository">Репозиторий проектов</param>
public class AddTaskIdToProjectTaskIdList<TEntity>(IProjectRepository<TEntity, int> repository) : IAddTaskIdToProjectTaskIdList
    where TEntity : IBaseEntity<int>
{
    private readonly IProjectRepository<TEntity, int> _repository = repository;

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
