using API.Dtos;
using AutoMapper;
using Core.Models.Order;

namespace API.Helpers
{
    public class OrderProductUrlResolver : IValueResolver<OrderProduct, OrderProductDto, string>
    {
        private readonly IConfiguration _config;

        public OrderProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(OrderProduct source, OrderProductDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProductOrdered.PictureUrl))
            {
                return _config["ApiUrl"] + source.ProductOrdered.PictureUrl;
            }

            return null;
        }
    }
}
