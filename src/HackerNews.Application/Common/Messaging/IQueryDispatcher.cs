namespace HackerNews.Application.Common.Messaging;

public interface IQueryDispatcher
{
    Task<TResult> Dispatch<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>;
}

