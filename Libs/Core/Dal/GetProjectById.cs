using Core.Dal.Base;
using Core.Exceptions;

namespace Core.Dal;

/// <summary>
/// Реализация контракта IGetProjectById
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <param name="repository">Репозиторий проектов</param>
public class GetProjectById<TEntity>(IProjectRepository<TEntity, int> repository) : IGetProjectById<TEntity>
    where TEntity : IBaseEntity<int>
{
    private readonly IProjectRepository<TEntity, int> _repository = repository;

    public async Task<TEntity> GetById(int id)
    {
        var existingProject = await _repository.GetProjectByIdAsync(id);

        if (existingProject is TEntity model)
            return model ?? throw new ValidationException("Не удалось найти существующий проектв БД", string.Empty);

        throw new Exception($"{typeof(TEntity)} не является типом необходимого проекта");
    }
}
