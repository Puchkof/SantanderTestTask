namespace HackerNews.Application.Common.Messaging;

public interface ICachedQuery
{
    string CacheKey { get; }
    TimeSpan? CacheExpiration { get; }
}

