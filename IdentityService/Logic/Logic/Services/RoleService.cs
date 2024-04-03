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
    /// <inheritdoc/>
    public async Task<IList<RoleDto>> GetAllDtosAsync()
    {
        var roles = await unitOfWork.Roles.GetAllAsync();
        return mapper.Map<IList<RoleDto>>(roles);
    }

    /// <inheritdoc/>
    public async Task<RoleDto> GetDtoByIdAsync(int id)
    {
        var existingRole = await unitOfWork.Roles.GetByIdAsync(id);
        return existingRole == null
            ? throw new ValidationException("Роль не найдена в базе данных", string.Empty)
            : mapper.Map<RoleDto>(existingRole);
    }

    /// <inheritdoc/>
    public async Task<RoleDto> CreateDtoAsync(RoleDto dto)
    {
        var roleDal = mapper.Map<RoleDal>(dto);
        var createdRoleDal = await unitOfWork.Roles.CreateAsync(roleDal);
        return mapper.Map<RoleDto>(createdRoleDal);
    }

    /// <inheritdoc/>
    public async Task<RoleDto> DeleteDtoAsync(int id)
    {
        var existingRoleDto = await GetDtoByIdAsync(id);
        var existingRoleDal = mapper.Map<RoleDal>(existingRoleDto);
        var deletedRoleDal = await unitOfWork.Roles.DeleteAsync(existingRoleDal);

        return mapper.Map<RoleDto>(deletedRoleDal);
    }

    /// <inheritdoc/>
    public async Task<RoleDto> UpdateDtoAsync(RoleDto dto, int id)
    {
        var existingRoleDal = await unitOfWork.Roles.GetByIdAsync(id);

        if (existingRoleDal == null)
            throw new ValidationException("Роль не найдена в базе данных", string.Empty);

        var updatedRoleDal = new RoleDal
        {
            Id = dto.Id,
            Name = dto.Name,
            UserIds = dto.UserIds
        };

        existingRoleDal = updatedRoleDal;
        updatedRoleDal = await unitOfWork.Roles.UpdateAsync(existingRoleDal);
        return mapper.Map<RoleDto>(updatedRoleDal);
    }
}