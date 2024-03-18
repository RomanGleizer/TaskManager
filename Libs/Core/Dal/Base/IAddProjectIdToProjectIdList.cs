using Microsoft.AspNetCore.Identity;

namespace Core.Dal.Base;

public interface IAddProjectIdToProjectIdList
{
    /// <summary>
    /// Добавляет новый проект у пользователя
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="memberId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<IdentityResult> AddProjectIdToProjectIdListAsync(int projectId, Guid memberId);
}
