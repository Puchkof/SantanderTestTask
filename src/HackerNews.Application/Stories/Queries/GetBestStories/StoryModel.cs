namespace HackerNews.Application.Stories.Queries.GetBestStories;

public record StoryModel(
    string Title,
    string Uri,
    string PostedBy,
    DateTimeOffset Time,
    int Score,
    int CommentCount
);
