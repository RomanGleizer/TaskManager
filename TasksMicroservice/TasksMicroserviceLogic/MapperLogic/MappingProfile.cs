using AutoMapper;
using TasksMicroservice.TasksMicroserviceDal.Entities;
using TasksMicroservice.TasksMicroserviceLogic.Dto;

namespace TasksMicroservice.TasksMicroserviceLogic.MapperLogic;

/// <summary>
///     Профиль маппинга для сопоставления объектов между слоями DAL и DTO
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    ///     Конструктор профиля маппинга
    /// </summary>
    public MappingProfile()
    {
        CreateMap<TaskDal, TaskDto>();
        CreateMap<TaskDto, TaskDal>();
    }
}