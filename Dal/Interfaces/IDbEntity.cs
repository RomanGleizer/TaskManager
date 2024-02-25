namespace Dal.Interfaces;

/// <summary>
/// Интерфейс, представляющий сущность с идентификатором типа <typeparamref name="T"/>.
/// </summary>
public interface IDbEntity<T>
{
    /// <summary>
    /// Идентификатор сущности.
    /// </summary>
    T Id { get; set; }
}
