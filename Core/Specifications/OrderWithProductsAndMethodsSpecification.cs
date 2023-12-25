using Core.Models.Order;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class OrderWithProductsAndMethodsSpecification : BaseSpecification<Order>
    {
        public OrderWithProductsAndMethodsSpecification(string buyerEmail) : base(o => o.BuyerEmail == buyerEmail)   //for returning list of orders List<Order>
        {
            AddInclude(o => o.OrderProducts);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrderWithProductsAndMethodsSpecification(int orderId, string buyerEmail) : base(o => o.Id == orderId && o.BuyerEmail == buyerEmail)    //for returning individual orders
        {
            AddInclude(o => o.OrderProducts);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}
