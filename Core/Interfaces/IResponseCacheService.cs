namespace Core.Interfaces
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive); //cache responses into memory(Redis)
        Task<string> GetCachedResponseAsync(string cacheKey);   //get cached responses into memory(Redis)
    }
}
