namespace Core.Dal.Base;

/// <summary>
///     Интерфейс базового объекта передачи данных с идентификатором типа <typeparamref name="TId" />
/// </summary>
public interface IBaseDto<TId>
{
    /// <summary>
    ///     Получает или задает идентификатор объекта
    /// </summary>
    TId Id { get; init; }
}