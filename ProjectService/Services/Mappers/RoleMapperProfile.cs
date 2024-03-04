using AutoMapper;
using Domain.Entities;
using Services.ViewModels.RoleViewModels;

namespace Services.Mappers;

public class RoleMapperProfile : Profile
{
    public RoleMapperProfile()
    {
        CreateMap<Role, RoleViewModel>();
        CreateMap<RoleViewModel, Role>();
    }
}
