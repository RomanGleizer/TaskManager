using AutoMapper;
using Domain.Entities;
using Services.ViewModels.MemberViewModels;
using Services.ViewModels.ProjectViewModels;
using Services.ViewModels.RoleViewModels;

namespace Services.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Member, MemberViewModel>();
        CreateMap<Role, RoleViewModel>();
        CreateMap<RoleViewModel, Role>();
        CreateMap<Project, ProjectViewModel>();
        CreateMap<ProjectViewModel, Project>();
        CreateMap<CreateProjectViewModel, Project>();
        CreateMap<UpdateProjectViewModel, Project>();
    }
}
