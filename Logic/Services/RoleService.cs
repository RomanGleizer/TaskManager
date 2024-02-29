using AutoMapper;
using Dal.Entities;
using Dal.Interfaces;
using Logic.DTO;
using Logic.Infrastructure;
using Logic.Interfaces;

namespace Logic.Services;

/// <summary>
/// Сервис для работы с ролями в формате DTO
/// </summary>
public class RoleService : IDtoService<RoleDTO, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует новый экземпляр класса RoleService
    /// </summary>
    /// <param name="unitOfWork">Объект единицы работы</param>
    /// <param name="mapper">Объект маппера</param>
    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Получает все DTO ролей
    /// </summary>
    public IEnumerable<RoleDTO> GetAllDtos()
    {
        var allRoleDals = _unitOfWork.Roles.GetAll();
        return _mapper.Map<IEnumerable<RoleDal>, IEnumerable<RoleDTO>>(allRoleDals);
    }

    /// <summary>
    /// Асинхронно получает DTO роли по ее идентификатору
    /// </summary>
    /// <param name="roleId">Идентификатор роли</param>
    public async Task<RoleDTO> GetDtoByIdAsync(int roleId)
    {
        var roleDal = await GetExistingRoleAsync(roleId);
        return _mapper.Map<RoleDTO>(roleDal);
    }

    /// <summary>
    /// Асинхронно создает DTO роли.
    /// </summary>
    /// <param name="roleDTO">DTO роли для создания.</param>
    public async Task<RoleDTO> CreateDtoAsync(RoleDTO roleDTO)
    {
        var roleDal = _mapper.Map<RoleDal>(roleDTO);

        await _unitOfWork.Roles.CreateAsync(roleDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RoleDTO>(roleDal);
    }

    /// <summary>
    /// Асинхронно удаляет DTO роли по ее идентификатору
    /// </summary>
    /// <param name="roleId">Идентификатор роли для удаления</param>
    public async Task<RoleDTO> DeleteDtoAsync(int roleId)
    {
        var existingRoleDal = await GetExistingRoleAsync(roleId);

        _unitOfWork.Roles.Delete(existingRoleDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RoleDTO>(existingRoleDal);
    }

    /// <summary>
    /// Асинхронно обновляет DTO роли по ее идентификатору
    /// </summary>
    /// <param name="roleDTO">Обновленные данные DTO роли</param>
    /// <param name="roleId">Идентификатор роли для обновления</param>
    public async Task<RoleDTO> UpdateDtoAsync(RoleDTO roleDTO, int roleId)
    {
        var existingRoleDal = await GetExistingRoleAsync(roleId);

        existingRoleDal.Name = roleDTO.Name;
        existingRoleDal.UserIds = roleDTO.UserIds;

        foreach (var userId in roleDTO.UserIds)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user != null)
                existingRoleDal.Users.Add(user);
        }

        _unitOfWork.Roles.Update(existingRoleDal);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RoleDTO>(existingRoleDal);
    }

    /// <summary>
    /// Асинхронно получает существующую роль по ее идентификатору
    /// </summary>
    /// <param name="roleId">Идентификатор роли</param>
    /// <returns>Существующая роль.</returns>
    private async Task<RoleDal> GetExistingRoleAsync(int roleId)
    {
        var existingTaskDal = await _unitOfWork.Roles.GetByIdAsync(roleId);
        if (existingTaskDal == null)
            throw new ValidationException("Роль не найдена в базе данных", string.Empty);
        return existingTaskDal;
    }
}
