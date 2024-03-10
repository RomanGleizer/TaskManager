using AutoMapper;
using Domain.Entities;
using Services.ViewModels.ProjectViewModels;

namespace Services.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Project, ProjectViewModel>();
        CreateMap<ProjectViewModel, Project>();
        CreateMap<CreateProjectViewModel, Project>();
        CreateMap<UpdateProjectViewModel, Project>();
    }
}
