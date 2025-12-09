namespace HackerNews.Infrastructure.Configuration;

public class HackerNewsApiOptions
{
    public const string SectionName = "HackerNewsApi";

    public string BaseUrl { get; set; } = "https://hacker-news.firebaseio.com/v0";
    public int CacheDurationMinutes { get; set; } = 5;
}

