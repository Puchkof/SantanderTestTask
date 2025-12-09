using HackerNews.Domain.Repositories;
using HackerNews.Infrastructure.Repositories;
using HackerNews.IntegrationTests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HackerNews.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing HttpClient registration
            services.RemoveAll<IHttpClientFactory>();
            services.RemoveAll<IStoryRepository>();

            // Add mock HttpClient for HackerNews API
            services.AddHttpClient<IStoryRepository, HackerNewsStoryRepository>((sp, client) =>
            {
                client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0");
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new MockHackerNewsHttpMessageHandler());
        });
    }
}

