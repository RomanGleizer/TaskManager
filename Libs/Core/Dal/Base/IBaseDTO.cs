namespace Core.Dal.Base;

/// <summary>
/// Интерфейс базового объекта передачи данных с идентификатором типа <typeparamref name="T"/>
/// </summary>
public interface IBaseDTO<T>
{
    /// <summary>
    /// Получает или задает идентификатор объекта
    /// </summary>
    T Id { get; init; }
}
