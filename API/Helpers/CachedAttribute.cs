using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace API.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter    //IAsyncActionFilter = allow code to be run before or after an action method/endpoint  
    {
        private readonly int _timeToLiveSeconds;

        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>(); //get ResponseCacheService

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request); //generate cacheKey
            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);   //get cachedResponse

            if (!string.IsNullOrEmpty(cachedResponse))  //check if is NOT null
            {
                var contentResult = new ContentResult   //create ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = contentResult; //set ContentResult and send the cachedResponse/contentResult to the client
                return;
            }

            var executedContext = await next(); //let controller decide/execute code, move to controller

            if (executedContext.Result is OkObjectResult okObjectResult)    //if the controller executedContext.Result returned an OkObjectResult...
            {
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds)); //...put the response/result into cache/Redis, so the next time
                                                                                                                                //someone asks for the same thing, they can get it from cache
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request) //generate a key to identify in redisDb what is being requested to check if it exists to return it
        {
            //organize request query string in a very specific order to always have the same key, and that key will be used to always return the same response
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key)) //loop over all of the query strings, all over the keys and the values in the query string
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}
