using Services.Interfaces;
using Domain.Interfaces;
using Services.ViewModels.MemberViewModels;
using AutoMapper;

namespace Services.Services;

/// <summary>
/// Сервис для работы с ролями
/// </summary>
public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получает информацию о роли по её идентификатору
    /// </summary>
    /// <param name="id">Идентификатор роли</param>
    /// <returns>Модель представления роли или null, если роль не найдена</returns>
    public async Task<MemberViewModel?> GetById(int id)
    {
        var existingRole = await _repository.GetRoleByIdAsync(id);
        return _mapper.Map<MemberViewModel?>(existingRole);
    }
}