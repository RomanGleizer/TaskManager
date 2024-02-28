using AutoMapper;
using Dal.Entities;
using Dal.Interfaces;
using Logic.DTO;
using Logic.Infrastructure;
using Logic.Interfaces;

namespace Logic.Services;

public class ProjectService : IDtoService<ProjectDTO, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public IEnumerable<ProjectDTO> GetAllDtos()
    {
        var allProjectDals = _unitOfWork.Projects.GetAll();
        return _mapper.Map<IEnumerable<ProjectDal>, IEnumerable<ProjectDTO>>(allProjectDals);
    }

    public async Task<ProjectDTO> GetDtoByIdAsync(int id)
    {
        var projectDal = await _unitOfWork.Projects.GetByIdAsync(id);
        if (projectDal == null)
            throw new ValidationException("Project was not found in database", string.Empty);
        return _mapper.Map<ProjectDTO>(projectDal);
    }

    public async Task<ProjectDTO> CreateDtoAsync(ProjectDTO projectDTO)
    {
        var projectDal = _mapper.Map<ProjectDal>(projectDTO);

        await _unitOfWork.Projects.CreateAsync(projectDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProjectDTO>(projectDal);
    }

    public async Task<ProjectDTO> DeleteDtoAsync(int id)
    {
        var existingProjectDal = await _unitOfWork.Projects.GetByIdAsync(id);
        if (existingProjectDal == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        _unitOfWork.Projects.Delete(existingProjectDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProjectDTO>(existingProjectDal);
    }

    public async Task<ProjectDTO> UpdateDtoAsync(ProjectDTO projectDTO, int projectId)
    {
        var existingProjectDal = await _unitOfWork.Projects.GetByIdAsync(projectId);
        if (existingProjectDal == null)
            throw new ValidationException("Project was not found in database", string.Empty);

        existingProjectDal.Name = projectDTO.Name;
        existingProjectDal.Description = projectDTO.Description;
        existingProjectDal.CreationDate = projectDTO.CreationDate;
        existingProjectDal.LastModifidedDate = projectDTO.LastModifidedDate;
        existingProjectDal.ParticipantIds = projectDTO.ParticipantIds;
        existingProjectDal.TaskIds = projectDTO.TaskIds;

        // Id обновляем, а сами задачи и участников - нет.

        _unitOfWork.Projects.Update(existingProjectDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProjectDTO>(existingProjectDal);
    }
}
