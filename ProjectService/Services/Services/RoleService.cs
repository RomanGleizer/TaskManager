using Services.Interfaces;
using Domain.Interfaces;
using Services.ViewModels.MemberViewModels;
using AutoMapper;

namespace Services.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MemberViewModel?> GetById(int id)
    {
        var existingRole = await _repository.GetRoleByIdAsync(id);
        return _mapper.Map<MemberViewModel?>(existingRole);
    }
}
