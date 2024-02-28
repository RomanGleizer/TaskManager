using Dal.Interfaces;
using Logic.DTO;
using Logic.Interfaces;
using Logic.Infrastructure;
using AutoMapper;
using Dal.Entities;

namespace Logic.Services;

public class TaskService : IDtoService<TaskDTO, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TaskDTO> GetDtoByIdAsync(int id)
    {
        var taskDal = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (taskDal == null)
            throw new ValidationException("Task was not found in database", string.Empty);
        return _mapper.Map<TaskDTO>(taskDal);
    }

    public IEnumerable<TaskDTO> GetAllDtos()
    {
        var allTaskDals = _unitOfWork.Tasks.GetAll();
        return _mapper.Map<IEnumerable<TaskDal>, IEnumerable<TaskDTO>>(allTaskDals);
    }

    public async Task<TaskDTO> CreateDtoAsync(TaskDTO taskDTO)
    {
        // Логика проверки, зареган ли текущий пользователь.

        var taskDal = _mapper.Map<TaskDal>(taskDTO);

        await _unitOfWork.Tasks.CreateAsync(taskDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TaskDTO>(taskDal);
    }

    public async Task<TaskDTO> DeleteDtoAsync(int id)
    {
        var existingTaskDal = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (existingTaskDal == null)
            throw new ValidationException("Task was not found in database", string.Empty);

        _unitOfWork.Tasks.Delete(existingTaskDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TaskDTO>(existingTaskDal);
    }

    public async Task<TaskDTO> UpdateDtoAsync(TaskDTO taskDTO, int taskId)
    {
        var existingTaskDal = await _unitOfWork.Tasks.GetByIdAsync(taskId);
        if (existingTaskDal == null)
            throw new ValidationException("Task was not found in database", string.Empty);

        existingTaskDal.Name = taskDTO.Name;
        existingTaskDal.Description = taskDTO.Description;
        existingTaskDal.ExecutionStatus = taskDTO.ExecutionStatus;
        existingTaskDal.CreatedDate = taskDTO.CreatedDate;
        existingTaskDal.LastUpdateDate = taskDTO.LastUpdateDate;
        existingTaskDal.PerformerIds = taskDTO.PerformerIds;
        existingTaskDal.CommentIds = taskDTO.CommentIds;
        existingTaskDal.ProjectId = taskDTO.ProjectId;

        // Id обновляем, а сам проект и участников - нет.

        _unitOfWork.Tasks.Update(existingTaskDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TaskDTO>(existingTaskDal);
    }
}
