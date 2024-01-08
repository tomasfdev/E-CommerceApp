using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)   //put/store things into redis database for cache responses
        {
            if (response is null)
            {
                return;
            }

            var options = new JsonSerializerOptions //serialize json responses in CamelCase
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serialisedResponse = JsonSerializer.Serialize(response, options);   //serialize response

            await _database.StringSetAsync(cacheKey, serialisedResponse, timeToLive);   //write on redisDb
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey) //get things out of redis database for cache responses
        {
            var cachedResponse = await _database.StringGetAsync(cacheKey);

            if (cachedResponse.IsNullOrEmpty)
            {
                return null;
            }

            return cachedResponse;
        }
    }
}
