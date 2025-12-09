using System.Net;
using System.Text;
using System.Text.Json;

namespace HackerNews.IntegrationTests.Mocks;

public class MockHackerNewsHttpMessageHandler : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.RequestUri?.AbsolutePath.EndsWith("beststories.json") == true)
        {
            var storyIds = Enumerable.Range(1, 200).Select(i => (long)i).ToArray();
            var json = JsonSerializer.Serialize(storyIds);
            
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        if (request.RequestUri?.AbsolutePath.Contains("/item/") == true)
        {
            var idString = request.RequestUri.AbsolutePath.Split('/').Last().Replace(".json", "");
            if (long.TryParse(idString, out var id))
            {
                var story = new
                {
                    id,
                    title = $"Test Story {id}",
                    url = $"https://example.com/story/{id}",
                    by = $"user{id}",
                    time = DateTimeOffset.UtcNow.AddHours(-id).ToUnixTimeSeconds(),
                    score = 1000 - (int)id,
                    descendants = (int)id * 10
                };

                var json = JsonSerializer.Serialize(story);
                
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            }
        }

        return new HttpResponseMessage(HttpStatusCode.NotFound);
    }
}

