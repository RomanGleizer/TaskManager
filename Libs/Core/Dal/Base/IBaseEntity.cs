namespace Core.Dal.Base;

/// <summary>
/// Интерфейс, представляющий сущность с идентификатором типа <typeparamref name="T"/>
/// </summary>
public interface IBaseEntity<T>
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    T Id { get; init; }
}
