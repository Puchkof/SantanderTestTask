using FluentValidation;

namespace HackerNews.Application.Stories.Queries.GetBestStories;

public class GetBestStoriesQueryValidator : AbstractValidator<GetBestStoriesQuery>
{
    public GetBestStoriesQueryValidator()
    {
        RuleFor(x => x.Count)
            .GreaterThan(0)
            .WithMessage("Count must be greater than 0")
            .LessThanOrEqualTo(500)
            .WithMessage("Count must not exceed 500");
    }
}

