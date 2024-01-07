using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Order
{
    public class Order : BaseModel
    {
        public Order()
        {
        }

        public Order(IReadOnlyList<OrderProduct>? orderProducts, string? buyerEmail, Address? shipToAddress, DeliveryMethod? deliveryMethod, decimal subtotal, string? paymentIntentId)
        {
            OrderProducts = orderProducts;
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string? BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public virtual Address? ShipToAddress { get; set; }
        public virtual DeliveryMethod? DeliveryMethod { get; set; }
        public virtual IReadOnlyList<OrderProduct>? OrderProducts { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string? PaymentIntentId { get; set; }

        public decimal GetTotal() => Subtotal + DeliveryMethod.Price;
    }
}
