namespace Core.Dal.Base;

/// <summary>
///     Интерфейс добавления идентификатора задачи в список идентификаторов задач проекта
/// </summary>
public interface IAddTaskIdToProjectTaskIdList
{
    /// <summary>
    ///     Добавляет новую задачу в проект
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="taskId">Идентификатор добавляемой задачи</param>
    Task AddNewTaskIdInProjectIdList(Guid projectId, Guid taskId);
}