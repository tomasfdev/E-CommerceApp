using Core.Models.Order;

namespace API.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string? BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual Address? ShipToAddress { get; set; }
        public string? DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public virtual IReadOnlyList<OrderProductDto>? OrderProducts { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string? Status { get; set; }
    }
}
