using FluentValidation;
using HackerNews.Application.Common.Messaging;
using HackerNews.Application.Stories.Queries.GetBestStories;

namespace HackerNews.API.Endpoints;

public static class StoriesEndpoints
{
    public static IEndpointRouteBuilder MapStoriesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stories")
            .WithTags("Stories")
            .WithOpenApi();

        group.MapGet("/best/{count:int}", GetBestStories)
            .WithName("GetBestStories")
            .WithSummary("Get the best n stories from Hacker News")
            .WithDescription("Returns the best stories from Hacker News API, sorted by score in descending order")
            .Produces<IReadOnlyList<StoryModel>>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> GetBestStories(
        int count,
        IQueryDispatcher dispatcher,
        IValidator<GetBestStoriesQuery> validator,
        CancellationToken cancellationToken)
    {
        var query = new GetBestStoriesQuery(count);
        
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var stories = await dispatcher.Dispatch<GetBestStoriesQuery, IReadOnlyList<StoryModel>>(query, cancellationToken);
        return Results.Ok(stories);
    }
}

