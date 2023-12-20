using Core.Interfaces;
using Core.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();    //redis db connection
        }

        public Task<bool> DeleteBasketAsync(string BasketId)
        {
            return _database.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket> GetBasketByIdAsync(string BasketId)
        {
            var data = await _database.StringGetAsync(BasketId);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
            //se data for null retorna null else retorna data Deserialize em CustomerBasket em formato Json
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));   //cria baskter e permite duração de 30dias

            if (!created) return null;

            return await GetBasketByIdAsync(basket.Id);
        }
    }
}
