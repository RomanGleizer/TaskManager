using AutoMapper;
using Dal.Entities;
using Logic.DTO;

namespace Logic.MapperLogic;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TaskDal, TaskDTO>();
    }
}