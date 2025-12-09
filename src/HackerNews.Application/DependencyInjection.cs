using FluentValidation;
using HackerNews.Application.Common.Behaviors;
using HackerNews.Application.Common.Messaging;
using HackerNews.Application.Stories.Queries.GetBestStories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HackerNews.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

        services.AddScoped<GetBestStoriesQueryHandler>();
        services.TryAddScoped<IQueryHandler<GetBestStoriesQuery, IReadOnlyList<StoryModel>>>(sp =>
        {
            var handler = sp.GetRequiredService<GetBestStoriesQueryHandler>();
            var cacheService = sp.GetRequiredService<Application.Common.Caching.ICacheService>();
            return new CachedQueryHandler<GetBestStoriesQuery, IReadOnlyList<StoryModel>>(handler, cacheService);
        });

        services.AddScoped<IValidator<GetBestStoriesQuery>, GetBestStoriesQueryValidator>();

        return services;
    }
}

