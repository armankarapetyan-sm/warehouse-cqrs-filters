using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Warehouse.Caching;

namespace Warehouse.Filters;

public class ProductListCacheFilter : IResourceFilter
{
    private readonly ProductListCache _cache;

    public ProductListCacheFilter(ProductListCache cache)
    {
        _cache = cache;
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (context.HttpContext.Request.Method != HttpMethods.Get)
        {
            return;
        }

        object? cached;
        if (_cache.TryGet(out cached) && cached != null)
        {
            context.Result = new OkObjectResult(cached);
        }
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        if (context.Canceled || context.HttpContext.Request.Method != HttpMethods.Get)
        {
            return;
        }

        ObjectResult? result = context.Result as ObjectResult;
        if (result == null || result.Value == null)
        {
            return;
        }

        int status = result.StatusCode ?? 200;
        if (status != 200)
        {
            return;
        }

        _cache.Set(result.Value);
    }
}
