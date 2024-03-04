using AutoMapper;
using Domain.Entities;
using Services.ViewModels.ProjectViewModels;

namespace Services.Mappers;

public class ProjectMapperProfile : Profile
{
    public ProjectMapperProfile()
    {
        CreateMap<Project, ProjectViewModel>();
        CreateMap<ProjectViewModel, Project>();
    }
}
