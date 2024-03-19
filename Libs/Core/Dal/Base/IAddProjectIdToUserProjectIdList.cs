using Microsoft.AspNetCore.Identity;

namespace Core.Dal.Base;

/// <summary>
/// Добавляет идентификатора проекта в список идентификаторов проектов пользователя
/// </summary>
public interface IAddProjectIdToUserProjectIdList
{
    /// <summary>
    /// Добавляет новый проект у пользователя
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="memberId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<IdentityResult> AddProjectIdToProjectIdListAsync(int projectId, Guid memberId);
}
