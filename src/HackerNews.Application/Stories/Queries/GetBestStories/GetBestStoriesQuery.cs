using HackerNews.Application.Common.Messaging;

namespace HackerNews.Application.Stories.Queries.GetBestStories;

public record GetBestStoriesQuery(int Count) : IQuery<IReadOnlyList<StoryModel>>, ICachedQuery
{
    public string CacheKey => $"best-stories-{Count}";
    public TimeSpan? CacheExpiration => TimeSpan.FromMinutes(5);
}

