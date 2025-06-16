using Prodify.Models;
using AutoMapper;
using Prodify.Dtos.UserDto;
using Prodify.Dtos;

namespace Prodify.Mappers;

public class AuthMapper : Profile
{
    public AuthMapper()
    {
        CreateMap<User, UserProfileResponseDto>().ReverseMap();
    }
}
