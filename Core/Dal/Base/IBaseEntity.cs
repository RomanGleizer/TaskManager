namespace Core.Dal.Base;

/// <summary>
///     Интерфейс, представляющий сущность с идентификатором типа <typeparamref name="TId" />
/// </summary>
public interface IBaseEntity<TId>
{
    /// <summary>
    ///     Идентификатор сущности
    /// </summary>
    TId Id { get; init; }
}