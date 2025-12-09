using HackerNews.Application.Common.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.Infrastructure.Caching;

public sealed class MemoryCacheService(IMemoryCache cache) : ICacheService
{
    public async Task<T?> GetOrCreateAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        return await cache.GetOrCreateAsync(
            key,
            async entry =>
            {
                if (expiration.HasValue)
                {
                    entry.AbsoluteExpirationRelativeToNow = expiration.Value;
                }

                return await factory();
            });
    }
}

