using AutoMapper;
using Domain.Entities;
using Services.ViewModels;

namespace Services.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Project, ProjectViewModel>();
        CreateMap<ProjectViewModel, Project>();
    }
}
