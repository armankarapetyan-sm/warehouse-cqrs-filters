using Microsoft.Extensions.Caching.Memory;

namespace Warehouse.Caching;

public class ProductListCache
{
    public const string Key = "products:list";

    private readonly IMemoryCache _cache;

    public ProductListCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public bool TryGet(out object? value)
    {
        return _cache.TryGetValue(Key, out value);
    }

    public void Set(object value)
    {
        _cache.Set(Key, value, TimeSpan.FromMinutes(2));
    }

    public void Remove()
    {
        _cache.Remove(Key);
    }
}
