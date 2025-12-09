using HackerNews.Application.Common.Caching;
using HackerNews.Application.Common.Messaging;

namespace HackerNews.Application.Common.Behaviors;

public sealed class CachedQueryHandler<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> inner,
    ICacheService cacheService) : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>, ICachedQuery
{
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        var result = await cacheService.GetOrCreateAsync(
            query.CacheKey,
            () => inner.HandleAsync(query, cancellationToken),
            query.CacheExpiration,
            cancellationToken);

        return result!;
    }
}

