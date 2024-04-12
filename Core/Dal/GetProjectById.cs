using Core.Dal.Base;

namespace Core.Dal;

/// <summary>
///     Реализация контракта IGetProjectById
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <param name="repository">Репозиторий проектов</param>
public class GetProjectById<TEntity>(IProjectRepository<TEntity, Guid> repository) : IGetProjectById<TEntity>
    where TEntity : IBaseEntity<Guid>
{
    private readonly IProjectRepository<TEntity, Guid> _repository = repository;

    public async Task<TEntity> GetById(Guid id)
    {
        var existingProject = await _repository.GetProjectByIdAsync(id);

        if (existingProject is TEntity)
            return existingProject ?? throw new Exception("Не удалось найти существующий проектв БД");

        throw new Exception($"{typeof(TEntity)} не является типом необходимого проекта");
    }
}