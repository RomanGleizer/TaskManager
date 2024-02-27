using Dal.Interfaces;
using Logic.DTO;
using Logic.Interfaces;
using Logic.Infrastructure;
using AutoMapper;
using Dal.Entities;

namespace Logic.Services;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TaskDTO> GetTaskByIdAsync(int id)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null)
            throw new ValidationException("Task was not found in database", string.Empty);

        return new TaskDTO
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            ExecutionStatus = task.ExecutionStatus,
            CreatedDate = task.CreatedDate,
            LastUpdateDate = task.LastUpdateDate,
            PerformerIds = task.PerformerIds,
            ProjectId = task.ProjectId
        };
    }

    public IEnumerable<TaskDTO> GetAllTasks()
    {
        var allTasks = _unitOfWork.Tasks.GetAll();
        return _mapper.Map<IEnumerable<TaskDal>, IEnumerable<TaskDTO>>(allTasks);
    }

    public async Task<TaskDal> CreateTaskAsync(TaskDTO taskDTO)
    {
        // Логика проверки, зареган ли текущий пользователь.

        var task = new TaskDal
        {
            Name = taskDTO.Name,
            Description = taskDTO.Description,
            ExecutionStatus = taskDTO.ExecutionStatus,
            CreatedDate = taskDTO.CreatedDate,
            LastUpdateDate = taskDTO.LastUpdateDate,
            PerformerIds = taskDTO.PerformerIds,
            ProjectId = taskDTO.ProjectId
        };

        await _unitOfWork.Tasks.CreateAsync(task);
        await _unitOfWork.SaveChangesAsync();
        return task;
    }

    public async Task<TaskDal> DeleteTaskAsync(int id)
    {
        var existingTask = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (existingTask == null)
            throw new ValidationException("Task was not found in database", string.Empty);

        _unitOfWork.Tasks.Delete(existingTask);
        await _unitOfWork.SaveChangesAsync();

        return existingTask;
    }

    public async Task<TaskDal> UpdateTaskAsync(TaskDTO taskDTO, int taskId)
    {
        var existingTask = await _unitOfWork.Tasks.GetByIdAsync(taskId);
        if (existingTask == null)
            throw new ValidationException("Task was not found in database", string.Empty);

        existingTask.Name = taskDTO.Name;
        existingTask.Description = taskDTO.Description;
        existingTask.ExecutionStatus = taskDTO.ExecutionStatus;
        existingTask.CreatedDate = taskDTO.CreatedDate;
        existingTask.LastUpdateDate = taskDTO.LastUpdateDate;
        existingTask.PerformerIds = taskDTO.PerformerIds;
        existingTask.ProjectId = taskDTO.ProjectId;

        _unitOfWork.Tasks.Update(existingTask);
        await _unitOfWork.SaveChangesAsync();

        return existingTask;
    }
}
