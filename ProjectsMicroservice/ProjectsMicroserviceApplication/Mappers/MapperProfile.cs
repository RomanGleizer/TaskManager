using AutoMapper;
using ProjectsMicroservice.ProjectsMicroserviceApplication.ViewModels.ProjectViewModels;
using ProjectsMicroservice.ProjectsMicroserviceDomain.Entities;

namespace ProjectsMicroservice.ProjectsMicroserviceApplication.Mappers;

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