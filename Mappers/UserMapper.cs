using Prodify.Models;
using AutoMapper;
using Prodify.Dtos.UserDto;
using Prodify.Requests;
using Prodify.Dtos;

namespace Prodify.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, ListDto>().ReverseMap();
        CreateMap<User, DetailDto>().ReverseMap();
        CreateMap<User, SimpleDto>()
            .ReverseMap();
        CreateMap<User, CreateUserRequestDto>().ReverseMap();
        CreateMap<PaginatedResponseDto<User>, PaginatedResponseDto<ListDto>>()
            .ForMember(dest => dest.items, opt => opt.MapFrom(src => src.items));
    }
}
