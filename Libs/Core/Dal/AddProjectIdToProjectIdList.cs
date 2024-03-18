﻿using Core.Dal.Base;
using Core.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Core.Dal;

public class AddProjectIdToProjectIdList<TDal>(IUserRepository<TDal> userRepository) : IAddProjectIdToProjectIdList
    where TDal : IBaseEntity<Guid>
{
    private readonly IUserRepository<TDal> _userRepository = userRepository;

    public async Task<IdentityResult> AddProjectIdToProjectIdListAsync(int projectId, Guid memberId)
    {
        var existingUserDal = await _userRepository.GetByIdAsync(memberId)
            ?? throw new ValidationException("Пользователь не найден в базе данных", string.Empty);

        if (existingUserDal is IProjectsContainer container)
            container.ProjectIds.Add(projectId);

        var updatedUser = await _userRepository.UpdateAsync(existingUserDal)
            ?? throw new Exception("Произошла ошибка при обновлении проектов пользователя в базе данных");

        return updatedUser;
    }
}
