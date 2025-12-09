using HackerNews.Application.Common.Messaging;
using HackerNews.Domain.Repositories;

namespace HackerNews.Application.Stories.Queries.GetBestStories;

public class GetBestStoriesQueryHandler : IQueryHandler<GetBestStoriesQuery, IReadOnlyList<StoryModel>>
{
    private readonly IStoryRepository _storyRepository;

    public GetBestStoriesQueryHandler(IStoryRepository storyRepository)
    {
        _storyRepository = storyRepository;
    }

    public async Task<IReadOnlyList<StoryModel>> HandleAsync(GetBestStoriesQuery query, CancellationToken cancellationToken)
    {
        var storyIds = await _storyRepository.GetBestStoryIdsAsync(cancellationToken);

        var storyTasks = storyIds
            .Select(id => _storyRepository.GetStoryByIdAsync(id, cancellationToken))
            .ToList();

        var stories = await Task.WhenAll(storyTasks);

        return stories
            .Where(s => s != null)
            .OrderByDescending(s => s!.Score)
            .Take(query.Count)
            .Select(s => new StoryModel(
                Title: s!.Title ?? string.Empty,
                Uri: s.Url ?? string.Empty,
                PostedBy: s.By ?? string.Empty,
                Time: DateTimeOffset.FromUnixTimeSeconds(s.Time),
                Score: s.Score,
                CommentCount: s.Descendants
            ))
            .ToList();
    }
}

