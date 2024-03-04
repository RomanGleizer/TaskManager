using AutoMapper;
using Services.ViewModels.MemberViewModels;

namespace Services.Mappers;

public class MemberMapperProfile : Profile
{
    public MemberMapperProfile()
    {
        CreateMap<Domain.Entities.Member, MemberViewModel>();
    }
}
