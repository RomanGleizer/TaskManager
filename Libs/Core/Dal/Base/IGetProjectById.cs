namespace Core.Dal.Base;

/// <summary>
/// Получение проекта из бд по его идентификатору
/// </summary>
/// <typeparam name="TModel">Тип возвращаемой модели</typeparam>
public interface IGetProjectById<TModel>
    where TModel : IBaseEntity<Guid>
{
    /// <summary>
    /// Получает информацию о проекте по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор проекта</param>
    /// <returns>Модель представления проекта или null, если проект не найден</returns>
    Task<TModel> GetById(Guid id);
}
