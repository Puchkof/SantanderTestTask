using HackerNews.API.Filters;

namespace HackerNews.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "Hacker News API",
                Version = "v1",
                Description = "RESTful API to retrieve the best stories from Hacker News"
            });
        });

        services.AddScoped(typeof(ValidationFilter<>));

        return services;
    }
}

