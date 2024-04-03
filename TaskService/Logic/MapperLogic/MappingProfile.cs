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
        CreateMap<TaskDal, TaskDto>();
        CreateMap<TaskDto, TaskDal>();
    }
}