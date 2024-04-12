using AutoMapper;
using UsersMicroservice.UsersMicroserviceDal.Entities;
using UsersMicroservice.UsersMicroserviceLogic.Dto.Role;
using UsersMicroservice.UsersMicroserviceLogic.Dto.User;

namespace UsersMicroservice.UsersMicroserviceLogic.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, UserDal>();
        CreateMap<UserDal, UserDto>();

        CreateMap<CreateUserDto, UserDal>();
        CreateMap<UpdateUserDto, UserDal>();

        CreateMap<RoleDto, RoleDal>();
    }
}