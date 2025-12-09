using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Application.Common.Messaging;

public sealed class QueryDispatcher(IServiceScopeFactory scopeFactory) : IQueryDispatcher
{
    public async Task<TResult> Dispatch<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        
        var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        
        return await handler.HandleAsync(query, cancellationToken);
    }
}

