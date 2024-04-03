using Core.Dal.Base;
using Core.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Core.Dal;

/// <summary>
/// Реализация контракта IAddProjectIdToProjectIdList
/// </summary>
/// <typeparam name="TDal">Тип сущности</typeparam>
/// <param name="userRepository">репозиторий пользователей</param>
public class AddProjectIdToUserProjectIdList<TDal>(IUserRepository<TDal> userRepository) : IAddProjectIdToUserProjectIdList
    where TDal : IBaseEntity<Guid>
{
    private readonly IUserRepository<TDal> _userRepository = userRepository;

    public async Task<IdentityResult> AddProjectIdToProjectIdListAsync(Guid projectId, Guid memberId)
    {
        var existingUserDal = await _userRepository.GetByIdAsync(memberId)
            ?? throw new ValidationException("Пользователь не найден в базе данных", string.Empty);

        if (existingUserDal is IProjectIdsContainer container)
            container.ProjectIds.Add(projectId);

        var updatedUser = await _userRepository.UpdateAsync(existingUserDal)
            ?? throw new Exception("Произошла ошибка при обновлении проектов пользователя в базе данных");

        return updatedUser;
    }
}
