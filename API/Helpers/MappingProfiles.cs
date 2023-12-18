using API.Dtos;
using AutoMapper;
using Core.Models;
using Core.Models.Identity;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(destination => destination.ProductBrandName, o=> o.MapFrom(source => source.ProductBrand.Name))
                .ForMember(destination => destination.ProductTypeName, o => o.MapFrom(source => source.ProductType.Name))
                .ForMember(d => d.PictureUrl, o=> o.MapFrom<ProductUrlResolver>())
                .ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
