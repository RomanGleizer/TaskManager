namespace Core.Dal.Base;

/// <summary>
/// Контракт, который обязывает реализовать функционал списка идентификторов задач
/// </summary>
public interface ITaskIdsContainer
{
    /// <summary>
    /// Список идентификторов задач
    /// </summary>
    IList<Guid> TaskIds { get; }
}
