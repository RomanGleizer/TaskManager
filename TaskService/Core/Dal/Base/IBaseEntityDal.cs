namespace Core.Dal.Base;

/// <summary>
/// Интерфейс, представляющий сущность с идентификатором типа <typeparamref name="T"/>
/// </summary>
public interface IBaseEntityDal<T>
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    T Id { get; set; }
}
