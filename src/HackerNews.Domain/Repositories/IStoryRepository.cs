using HackerNews.Domain.Entities;

namespace HackerNews.Domain.Repositories;

public interface IStoryRepository
{
    Task<IReadOnlyList<long>> GetBestStoryIdsAsync(CancellationToken cancellationToken = default);
    Task<HackerNewsStory?> GetStoryByIdAsync(long id, CancellationToken cancellationToken = default);
}

