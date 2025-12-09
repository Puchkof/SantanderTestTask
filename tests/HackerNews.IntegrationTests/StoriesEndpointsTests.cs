using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using HackerNews.Application.Stories.Queries.GetBestStories;

namespace HackerNews.IntegrationTests;

public class StoriesEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public StoriesEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetBestStories_WithValidCount_ReturnsOkWithStories()
    {
        // Arrange
        const int count = 10;

        // Act
        var response = await _client.GetAsync($"/api/stories/best/{count}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var stories = await response.Content.ReadFromJsonAsync<List<StoryModel>>();
        stories.Should().NotBeNull();
        stories.Should().HaveCountLessOrEqualTo(count);
        
        // Verify stories are sorted by score descending
        if (stories!.Count > 1)
        {
            for (int i = 0; i < stories.Count - 1; i++)
            {
                stories[i].Score.Should().BeGreaterOrEqualTo(stories[i + 1].Score);
            }
        }

        // Verify story structure
        foreach (var story in stories)
        {
            story.Title.Should().NotBeNullOrEmpty();
            story.PostedBy.Should().NotBeNullOrEmpty();
            story.Score.Should().BeGreaterOrEqualTo(0);
            story.CommentCount.Should().BeGreaterOrEqualTo(0);
        }
    }

    [Fact]
    public async Task GetBestStories_WithCountOne_ReturnsSingleStory()
    {
        // Arrange
        const int count = 1;

        // Act
        var response = await _client.GetAsync($"/api/stories/best/{count}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var stories = await response.Content.ReadFromJsonAsync<List<StoryModel>>();
        stories.Should().NotBeNull();
        stories.Should().HaveCountLessOrEqualTo(count);
    }

    [Fact]
    public async Task GetBestStories_WithZeroCount_ReturnsBadRequest()
    {
        // Arrange
        const int count = 0;

        // Act
        var response = await _client.GetAsync($"/api/stories/best/{count}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetBestStories_WithNegativeCount_ReturnsBadRequest()
    {
        // Arrange
        const int count = -1;

        // Act
        var response = await _client.GetAsync($"/api/stories/best/{count}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetBestStories_WithCountExceedingLimit_ReturnsBadRequest()
    {
        // Arrange
        const int count = 501;

        // Act
        var response = await _client.GetAsync($"/api/stories/best/{count}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetBestStories_MultipleCalls_ReturnsCachedResults()
    {
        // Arrange
        const int count = 5;

        // Act - Make two calls
        var response1 = await _client.GetAsync($"/api/stories/best/{count}");
        var response2 = await _client.GetAsync($"/api/stories/best/{count}");

        // Assert
        response1.StatusCode.Should().Be(HttpStatusCode.OK);
        response2.StatusCode.Should().Be(HttpStatusCode.OK);

        var stories1 = await response1.Content.ReadFromJsonAsync<List<StoryModel>>();
        var stories2 = await response2.Content.ReadFromJsonAsync<List<StoryModel>>();

        // Results should be the same (cached)
        stories1.Should().BeEquivalentTo(stories2);
    }
}

