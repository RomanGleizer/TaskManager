using Domain.Entities;
using Domain.Interfaces;
using Domain.Updated;
using Services.Interfaces.Project_Interfaces;

namespace Services;

public class UpdateProject : IUpdateProject
{
    private readonly IStoreProject _storeProject;
    private readonly IStoreMember _storeMember;

    public UpdateProject(IStoreProject storeProject, IStoreMember storeMember)
    {
        _storeProject = storeProject;
        _storeMember = storeMember;
    }

    public async Task<int> UpdateProjectAsync(int projectId, UpdateProjectData projectData)
    {
        var existingProject = await _storeProject.GetProjectByIdAsync(projectId);
        // Валидация (если не найден, бросаем исключение);

        existingProject = existingProject with
        {
            Name = projectData.Name,
            Description = projectData.Description,
            LastUpdatedDate = projectData.LastUpdatedDate,
            MemberIds = projectData.MemberIds
        };

        var updatedMembers = new List<Member>();
        foreach (var memberId in projectData.MemberIds)
        {
            var member = await _storeMember.GetMemberByIdAsync(memberId);
            updatedMembers.Add(member);
        }

        existingProject = existingProject with
        {
            Members = updatedMembers
        };

        return await _storeProject.UpdateProjectAsync(projectId, existingProject);
    }
}
