using AutoMapper;
using Dal.Entities;
using Logic.DTO;

namespace Logic.MapperLogic;

/// <summary>
/// Профиль маппинга для сопоставления объектов между слоями DAL и DTO
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Конструктор профиля маппинга
    /// </summary>
    public MappingProfile()
    {
        CreateMap<TaskDal, TaskDTO>();
        CreateMap<TaskDTO, TaskDal>();
        CreateMap<ProjectDal, ProjectDTO>();
        CreateMap<ProjectDTO, ProjectDal>();
        CreateMap<CommentDal, CommentDTO>();
        CreateMap<CommentDTO, CommentDTO>();
    }
}