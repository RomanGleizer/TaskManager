using Core.Dal.Base;

namespace Core.Dal;

/// <summary>
/// Реализация контракта IAddTaskIdToProjectIdList
/// </summary>
/// <param name="repository">Репозиторий проектов</param>
public class AddTaskIdToProjectTaskIdList<TEntity>(IProjectRepository<TEntity, Guid> repository) : IAddTaskIdToProjectTaskIdList
    where TEntity : IBaseEntity<Guid>
{
    private readonly IProjectRepository<TEntity, Guid> _repository = repository;

    public async Task AddNewTaskIdInProjectIdList(Guid projectId, int taskId)
    {
        var existingProject = await _repository.GetProjectByIdAsync(projectId);

        if (existingProject is ITaskIdsContainer container && !container.TaskIds.Contains(taskId))
        {
            container.TaskIds.Add(taskId);
            await _repository.SaveChangesAsync();
        }
    }
}
