using Prodify.Models;
using AutoMapper;
using Prodify.Dtos.ProductDto;
using Prodify.Dtos;

namespace Prodify.Mappers;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ListDto>().ReverseMap();
        CreateMap<Product, DetailDto>().ReverseMap();
        CreateMap<Product, SimpleDto>()
            .ReverseMap();
        CreateMap<Product, UpdateProductRequestDto>().ReverseMap();
        CreateMap<Product, CreateProductRequestDto>().ReverseMap();
        CreateMap<PaginatedResponseDto<Product>, PaginatedResponseDto<ListDto>>()
            .ForMember(dest => dest.items, opt => opt.MapFrom(src => src.items));
    }
}
