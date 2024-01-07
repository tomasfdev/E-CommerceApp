namespace Core.Models
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {
        }

        public CustomerBasket(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public int? DeliveryMethodId { get; set; }  //optional because at the time of adding products to basket there isn't the option to select the delivery method until the checkout
        public string? ClientSecret { get; set; }   //used by stripe to user confirm the payment intent 
        public string? PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
