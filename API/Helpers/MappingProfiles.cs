using API.Dtos;
using AutoMapper;
using Core.Models;
using Core.Models.Order;

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
            CreateMap<Core.Models.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ReverseMap();
            CreateMap<OrderProduct, OrderProductDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductOrdered.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ProductOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ProductOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderProductUrlResolver>())
                .ReverseMap();
        }
    }
}
