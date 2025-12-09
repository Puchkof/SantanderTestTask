using HackerNews.Application.Common.Caching;
using HackerNews.Domain.Repositories;
using HackerNews.Infrastructure.Caching;
using HackerNews.Infrastructure.Configuration;
using HackerNews.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<HackerNewsApiOptions>(
            configuration.GetSection(HackerNewsApiOptions.SectionName));

        services.AddMemoryCache();
        services.AddSingleton<ICacheService, MemoryCacheService>();

        services.AddHttpClient<IStoryRepository, HackerNewsStoryRepository>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<HackerNewsApiOptions>>();
            client.BaseAddress = new Uri(options.Value.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return services;
    }
}

