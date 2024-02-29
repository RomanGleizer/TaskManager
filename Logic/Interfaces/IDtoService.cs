using Core.Dal.Base;

namespace Logic.Interfaces;

/// <summary>
/// Интерфейс сервиса объектов DTO
/// </summary>
/// <typeparam name="T">Тип объекта DTO</typeparam>
/// <typeparam name="I">Тип идентификатора объекта</typeparam>
public interface IDtoService<T, I> 
    where T : IBaseDTO<I>
{
    /// <summary>
    /// Получает объект DTO по идентификатору асинхронно
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    Task<T> GetDtoByIdAsync(I id);

    /// <summary>
    /// Получает все объекты DTO
    /// </summary>
    IEnumerable<T> GetAllDtos();

    /// <summary>
    /// Создает объект DTO асинхронно
    /// </summary>
    /// <param name="dto">DTO объект для создания</param>
    Task<T> CreateDtoAsync(T dto);

    /// <summary>
    /// Удаляет объект DTO асинхронно по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор объекта для удаления</param>
    Task<T> DeleteDtoAsync(I id);

    /// <summary>
    /// Обновляет объект DTO асинхронно
    /// </summary>
    /// <param name="dto">DTO объект для обновления</param>
    /// <param name="id">Идентификатор объекта для обновления</param>
    Task<T> UpdateDtoAsync(T dto, I id);
}
