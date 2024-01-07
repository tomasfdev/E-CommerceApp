using Core.Interfaces;
using Core.Models;
using Core.Models.Order;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Models.Product;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        public PaymentService(IBasketRepository basketRepository, IUnitOfWork uow, IConfiguration config)
        {
            _basketRepository = basketRepository;
            _uow = uow;
            _config = config;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];   //get apiKey

            var basket = await _basketRepository.GetBasketByIdAsync(basketId);  //get basket

            if (basket is null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)   //check if there is a basket
            {
                var deliveryMethod = await _uow.Repository<DeliveryMethod>().GetByIdAsync((int)basket.DeliveryMethodId);    //get deliveryMethod selected from basket
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.Items)
            {
                var product = await _uow.Repository<Product>().GetByIdAsync(item.Id);   //get product from db to confirm price
                if (item.Price != product.Price)    //check price
                {
                    item.Price = product.Price; //set real price
                }
            }

            var paymentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))   //check if there is a PaymentIntentId
            {
                var options = new PaymentIntentCreateOptions    //creates PaymentIntentOptions
                {
                    Amount = (long)basket.Items.Sum(product => product.Quantity * (product.Price * 100)) + (long)shippingPrice * 100,   //set amount prop
                    Currency = "usd",   //set Currency prop
                    PaymentMethodTypes = new List<string> { "card" }    //set PaymentMethodTypes prop
                };
                paymentIntent = await paymentService.CreateAsync(options);  //creates paymentIntent
                basket.PaymentIntentId = paymentIntent.Id;  //update basket.PaymentIntentId
                basket.ClientSecret = paymentIntent.ClientSecret;   //update basket.ClientSecret
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.Items.Sum(product => product.Quantity * (product.Price * 100)) + (long)shippingPrice * 100
                };
                await paymentService.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.UpdateBasketAsync(basket);  //update basket

            return basket;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpec(paymentIntentId);
            var order = await _uow.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (order is null) return null;

            order.Status = OrderStatus.PaymentFailed;
            await _uow.Complete();

            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpec(paymentIntentId);
            var order = await _uow.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (order is null) return null;

            order.Status = OrderStatus.PaymentReceived;
            await _uow.Complete();

            return order;
        }
    }
}
