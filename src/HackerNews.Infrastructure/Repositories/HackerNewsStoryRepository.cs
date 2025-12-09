using System.Net.Http.Json;
using HackerNews.Domain.Entities;
using HackerNews.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace HackerNews.Infrastructure.Repositories;

public sealed class HackerNewsStoryRepository(
    HttpClient httpClient,
    ILogger<HackerNewsStoryRepository> logger) : IStoryRepository
{
    public async Task<IReadOnlyList<long>> GetBestStoryIdsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching best story IDs from Hacker News API");

        var storyIds = await httpClient.GetFromJsonAsync<long[]>(
            "beststories.json",
            cancellationToken);

        return storyIds ?? [];
    }

    public async Task<HackerNewsStory?> GetStoryByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogDebug("Fetching story {StoryId} from Hacker News API", id);

            var story = await httpClient.GetFromJsonAsync<HackerNewsStory>(
                $"item/{id}.json",
                cancellationToken);

            return story;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching story {StoryId}", id);
            return null;
        }
    }
}
