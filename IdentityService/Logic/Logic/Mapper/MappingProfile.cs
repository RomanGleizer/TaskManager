using AutoMapper;
using Dal.Entities;
using Logic.Dto.Role;
using Logic.Dto.User;

namespace Logic.Mapper;

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