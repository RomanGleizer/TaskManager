using AutoMapper;
using Core.Dal.Base;
using Dal.Interfaces;
using Core.Exceptions;
using Logic.Dto;
using Dal.Entities;

namespace Logic.Services;

/// <summary>
/// Сервисный класс для управления объектами RoleDTO
/// </summary>
public class RoleService : IDtoService<RoleDTO, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RoleService"/>
    /// </summary>
    /// <param name="unitOfWork">UnitOfWork</param>
    /// <param name="mapper">Маппер</param>
    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<IList<RoleDTO>> GetAllDtosAsync()
    {
        var roleDals = await _unitOfWork.Roles.GetAllAsync();
        return _mapper.Map<IList<RoleDTO>>(roleDals);
    }

    /// <inheritdoc/>
    public async Task<RoleDTO?> GetDtoByIdAsync(int id)
    {
        var existingRole = await _unitOfWork.Roles.GetByIdAsync(id);
        if (existingRole == null)
            throw new ValidationException("Роль не найдена в базе данных", string.Empty);

        return _mapper.Map<RoleDTO>(existingRole);
    }

    /// <inheritdoc/>
    public async Task<RoleDTO?> CreateDtoAsync(RoleDTO dto)
    {
        var roleDal = _mapper.Map<RoleDal>(dto);
        var createdRoleDal = await _unitOfWork.Roles.CreateAsync(roleDal);
        return _mapper.Map<RoleDTO>(createdRoleDal);
    }

    /// <inheritdoc/>
    public async Task<RoleDTO?> DeleteDtoAsync(int id)
    {
        var existingRoleDto = await GetDtoByIdAsync(id);
        var existingRoleDal = _mapper.Map<RoleDal>(existingRoleDto);
        var deletedRoleDal = await _unitOfWork.Roles.DeleteAsync(existingRoleDal);

        return _mapper.Map<RoleDTO>(deletedRoleDal);
    }

    /// <inheritdoc/>
    public async Task<RoleDTO?> UpdateDtoAsync(RoleDTO dto, int id)
    {
        var existingRoleDal = await _unitOfWork.Roles.GetByIdAsync(id);

        if (existingRoleDal == null)
            throw new ValidationException("Роль не найдена в базе данных", string.Empty);

        var updatedRoleDal = new RoleDal
        {
            Id = dto.Id,
            Name = dto.Name,
            UserIds = dto.UserIds
        };

        existingRoleDal = updatedRoleDal;
        updatedRoleDal = await _unitOfWork.Roles.UpdateAsync(existingRoleDal);
        return _mapper.Map<RoleDTO>(updatedRoleDal);
    }
}