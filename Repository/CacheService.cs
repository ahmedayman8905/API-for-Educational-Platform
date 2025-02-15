using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Api_1.Repository;

public class CacheService (IDistributedCache distributedCache, ILogger<CacheService> logger)
{
    private readonly IDistributedCache distributedCache = distributedCache;

    // private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly ILogger<CacheService> _logger = logger;

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("Get cache with key: {key}", key);

        var cachedValue = await distributedCache.GetStringAsync(key, cancellationToken);

        //cachedValue is null
        return cachedValue is null
            ? null
            : JsonSerializer.Deserialize<T>(cachedValue);
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("Set cache with key: {key}", key);

        await distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Remove cache with key: {key}", key);

        await distributedCache.RemoveAsync(key, cancellationToken);
    }
}
