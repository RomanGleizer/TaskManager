using Logic.DTO;

namespace Logic.Interfaces;

public interface IProjectTaskService
{
    Task<ProjectTaskDTO> GetTaskByIdAsync(int id);
    IEnumerable<ProjectTaskDTO> GetAllTasks();
    Task CreateTaskAsync(ProjectTaskDTO task);
    Task DeleteTaskAsync(int id);
    Task UpdateTaskAsync(ProjectTaskDTO task, int id);
}
