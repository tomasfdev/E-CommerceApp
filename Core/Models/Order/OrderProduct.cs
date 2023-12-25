namespace Core.Models.Order
{
    public class OrderProduct : BaseModel
    {
        public OrderProduct()
        { 
        }

        public OrderProduct(ProductOrdered productOrdered, decimal price, int quantity)
        {
            this.ProductOrdered = productOrdered;
            Price = price;
            Quantity = quantity;
        }

        public virtual ProductOrdered? ProductOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
