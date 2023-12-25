using Core.Interfaces;
using Core.Models;
using Core.Models.Order;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IBasketRepository _basketRepo;

        public OrderService(IUnitOfWork uow, IBasketRepository basketRepo)
        {
            _uow = uow;
            _basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string baskedId, Address shipToAddress)
        {
            var basket = await _basketRepo.GetBasketByIdAsync(baskedId);    //get basket

            var products = new List<OrderProduct>();
            foreach (var item in basket.Items)  //percorre products(item) dentro do basket(basket.Items)
            {
                var product = await _uow.Repository<Product>().GetByIdAsync(item.Id); //vai à db buscar product que está dentro do basket(basket.Items)
                var productOrdered = new ProductOrdered(product.Id, product.Name, product.PictureUrl);  //cria produto encomendado(productOrdered)
                var orderProduct = new OrderProduct(productOrdered, product.Price, item.Quantity);  //produto encomendado(orderProduct)
                products.Add(orderProduct); //adiciona produto encomendado final
            }

            var deliveryMethod = await _uow.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            //if (deliveryMethod == null) return null;

            var subtotal = products.Sum(product => product.Price * product.Quantity);

            var order = new Order(products, buyerEmail, shipToAddress, deliveryMethod, subtotal);   //create order

            _uow.Repository<Order>().Add(order);    //create order

            var result = await _uow.Complete(); //save to db

            if (result <= 0) return null;   //if didn't save to db return null and let the order controller send the error response

            await _basketRepo.DeleteBasketAsync(baskedId);  //delete basket because order is saved/complete

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _uow.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var exp = new OrderWithProductsAndMethodsSpecification(id, buyerEmail);

            return await _uow.Repository<Order>().GetEntityWithSpecAsync(exp);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var exp = new OrderWithProductsAndMethodsSpecification(buyerEmail);

            return await _uow.Repository<Order>().GetAllWithSpecAsync(exp);
        }
    }
}
