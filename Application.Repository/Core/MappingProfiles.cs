using Application.Repository.DTO.Admin;
using Application.Repository.DTO.Common;
using Application.Repository.DTO.User;
using Application.Repository.Entities;
using AutoMapper;

namespace Application.Repository.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserDTO, User>().ReverseMap();

            CreateMap<UserUpdateDTO, User>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());

            CreateMap<AdminUserCreateDTO, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());

        }
    }
}
