namespace Core.Dal.Base;

/// <summary>
/// Контракт, который обязывает реализовать функционал списка идентификторов проектов
/// </summary>
public interface IProjectIdsContainer
{
    /// <summary>
    /// Список идентификаторов проектов
    /// </summary>
    IList<int> ProjectIds { get; }
}
