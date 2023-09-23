using Application.Repository.DTO.Admin;
using Application.Repository.DTO.User;
using Application.Repository.Entities;
using AutoMapper;
using Microsoft.VisualBasic;

namespace Application.Repository.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserDTO, User>().ReverseMap();

            CreateMap<UserUpdateDTO, User>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
