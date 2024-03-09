using AutoMapper;
using Core.Dal.Base;
using Core.Exceptions;
using Dal.Entities;
using Dal.Interfaces;
using Logic.Dto.Role;

namespace Logic.Services;

/// <summary>
/// Сервисный класс для управления объектами RoleDTO
/// </summary>
/// <remarks>
/// Инициализирует новый экземпляр класса <see cref="RoleService"/>
/// </remarks>
/// <param name="unitOfWork">UnitOfWork</param>
/// <param name="mapper">Маппер</param>
public class RoleService(IUnitOfWork unitOfWork, IMapper mapper) : IDtoService<RoleDto, int>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task<IList<RoleDto>> GetAllDtosAsync()
    {
        var roleDals = await _unitOfWork.Roles.GetAllAsync();
        return _mapper.Map<IList<RoleDto>>(roleDals);
    }

    /// <inheritdoc/>
    public async Task<RoleDto?> GetDtoByIdAsync(int id)
    {
        var existingRole = await _unitOfWork.Roles.GetByIdAsync(id);
        return existingRole == null
            ? throw new ValidationException("Роль не найдена в базе данных", string.Empty)
            : _mapper.Map<RoleDto>(existingRole);
    }

    /// <inheritdoc/>
    public async Task<RoleDto> CreateDtoAsync(RoleDto dto)
    {
        var roleDal = _mapper.Map<RoleDal>(dto);
        var createdRoleDal = await _unitOfWork.Roles.CreateAsync(roleDal);
        return _mapper.Map<RoleDto>(createdRoleDal);
    }

    /// <inheritdoc/>
    public async Task<RoleDto?> DeleteDtoAsync(int id)
    {
        var existingRoleDto = await GetDtoByIdAsync(id);
        var existingRoleDal = _mapper.Map<RoleDal>(existingRoleDto);
        var deletedRoleDal = await _unitOfWork.Roles.DeleteAsync(existingRoleDal);

        return _mapper.Map<RoleDto>(deletedRoleDal);
    }

    /// <inheritdoc/>
    public async Task<RoleDto?> UpdateDtoAsync(RoleDto dto, int id)
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
        return _mapper.Map<RoleDto>(updatedRoleDal);
    }
}