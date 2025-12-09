# Hacker News Best Stories API

A RESTful API built with ASP.NET Core that retrieves the best stories from the Hacker News API, sorted by score in descending order.

## Architecture

This solution follows **Clean Architecture** principles with clear separation of concerns:

```
├── src/
│   ├── HackerNews.Domain/          # Domain entities and interfaces
│   ├── HackerNews.Application/     # Business logic, queries, and validation
│   ├── HackerNews.Infrastructure/  # External services and data access
│   └── HackerNews.API/            # API endpoints and presentation layer
└── tests/
    └── HackerNews.IntegrationTests/ # Integration tests
```

### Key Design Patterns

- **Clean Architecture**: Separation of concerns with dependency inversion
- **CQS (Command Query Separation)**: Queries are separated from commands
- **Repository Pattern**: Abstraction over data access
- **Options Pattern**: Type-safe configuration using IOptions<T>
- **Minimal API**: Modern ASP.NET Core endpoint routing
- **Simple MediatR-like Dispatcher**: Generic-based query dispatcher without reflection overhead

## Features

- ✅ Retrieves best stories from Hacker News API
- ✅ Returns stories sorted by score (descending)
- ✅ In-memory caching to prevent API overload (configurable duration)
- ✅ FluentValidation with endpoint filters
- ✅ Swagger/OpenAPI documentation
- ✅ Comprehensive integration tests
- ✅ Structured logging
- ✅ Type-safe configuration

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## How to Run

### 1. Clone the repository

```bash
git clone <repository-url>
cd SantanderTestTask
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Run the application

```bash
dotnet run --project src/HackerNews.API/HackerNews.API.csproj
```

The API will start on `https://localhost:5001` (or `http://localhost:5000`).

### 4. Access Swagger UI

Open your browser and navigate to:
```
https://localhost:5001/swagger
```

### 5. Test the API

**Get best 10 stories:**
```bash
curl https://localhost:5001/api/stories/best/10
```

**Example response:**
```json
[
  {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "postedBy": "ismaildonmez",
    "time": "2019-10-12T13:43:01+00:00",
    "score": 1716,
    "commentCount": 572
  },
  ...
]
```

## Running Tests

### Run all tests

```bash
dotnet test
```

### Run tests with detailed output

```bash
dotnet test --logger "console;verbosity=detailed"
```

### Run tests with coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Configuration

Configuration is managed through `appsettings.json`:

```json
{
  "HackerNewsApi": {
    "BaseUrl": "https://hacker-news.firebaseio.com/v0",
    "CacheDurationMinutes": 5
  }
}
```

**Configuration Options:**

- `BaseUrl`: Hacker News API base URL
- `CacheDurationMinutes`: Duration to cache API responses (default: 5 minutes)

## API Endpoints

### GET /api/stories/best/{count}

Retrieves the best N stories from Hacker News.

**Parameters:**
- `count` (path, integer, required): Number of stories to retrieve (1-500)

**Responses:**
- `200 OK`: Returns array of stories
- `400 Bad Request`: Invalid count parameter (validation error)

**Validation Rules:**
- Count must be greater than 0
- Count must not exceed 500

## Project Structure

### Domain Layer (`HackerNews.Domain`)
- **Entities**: `Story`, `HackerNewsStory`
- **Interfaces**: `IStoryRepository`

### Application Layer (`HackerNews.Application`)
- **Messaging**: Simple MediatR-like implementation using generics
  - `IQuery<TResponse>`: Query marker interface
  - `IQueryHandler<TQuery, TResponse>`: Query handler interface
  - `IQueryDispatcher`: Query dispatcher
- **Queries**: `GetBestStoriesQuery` with `GetBestStoriesQueryHandler`
- **Validation**: FluentValidation validators
- **DTOs**: `StoryDto`

### Infrastructure Layer (`HackerNews.Infrastructure`)
- **Repositories**: `HackerNewsStoryRepository` with caching
- **Configuration**: `HackerNewsApiOptions`
- **HttpClient**: Configured with typed client pattern

### API Layer (`HackerNews.API`)
- **Endpoints**: Minimal API endpoints in `StoriesEndpoints`
- **Filters**: `ValidationFilter<T>` for FluentValidation integration
- **Extensions**: Service registration extensions

## Assumptions

1. **Caching Strategy**: 
   - In-memory caching is used to prevent overloading the Hacker News API
   - Cache duration is configurable (default: 5 minutes)
   - Both story IDs and individual stories are cached separately

2. **Error Handling**:
   - If a story fails to load, it's logged and excluded from results
   - The API continues processing other stories

3. **Validation**:
   - Maximum count is limited to 500 to prevent excessive API calls
   - Minimum count is 1

4. **Sorting**:
   - Stories are sorted by score in descending order
   - Sorting happens after fetching all stories

5. **Parallel Processing**:
   - Story details are fetched in parallel using `Task.WhenAll`
   - This significantly improves performance for large requests

## Enhancements & Future Improvements

Given more time, the following enhancements could be made:

### Performance & Scalability
1. **Distributed Caching**: Replace in-memory cache with Redis for multi-instance deployments
2. **Rate Limiting**: Add rate limiting to protect the API from abuse
3. **Pagination**: Implement cursor-based pagination for large result sets
4. **Bulk Fetching**: Optimize by batching story requests

### Resilience
5. **Retry Policies**: Add Polly for retry logic with exponential backoff
6. **Circuit Breaker**: Implement circuit breaker pattern for external API calls
7. **Health Checks**: Add health check endpoints for monitoring
8. **Graceful Degradation**: Return partial results if some stories fail to load

### Observability
9. **Structured Logging**: Enhanced logging with correlation IDs
10. **Metrics**: Add Prometheus metrics for monitoring
11. **Distributed Tracing**: Implement OpenTelemetry for tracing
12. **Application Insights**: Integration with Azure Application Insights

### Testing
13. **Unit Tests**: Add unit tests for individual components
14. **Load Tests**: Performance testing with k6 or JMeter
15. **Contract Tests**: API contract testing with Pact

### Features
16. **Filtering**: Add filters by date range, minimum score, etc.
17. **Search**: Full-text search capabilities
18. **Webhooks**: Notify subscribers of new top stories
19. **GraphQL**: Alternative GraphQL endpoint

### Security
20. **Authentication**: Add API key or JWT authentication
21. **CORS**: Configure CORS policies
22. **Input Sanitization**: Additional security measures

### DevOps
23. **Docker**: Containerization with Docker
24. **CI/CD**: GitHub Actions or Azure DevOps pipelines
25. **Infrastructure as Code**: Terraform or Bicep templates
26. **Kubernetes**: K8s deployment manifests

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Language**: C# 12
- **Validation**: FluentValidation
- **Testing**: xUnit, FluentAssertions, Microsoft.AspNetCore.Mvc.Testing
- **Caching**: Microsoft.Extensions.Caching.Memory
- **HTTP Client**: Microsoft.Extensions.Http
- **API Documentation**: Swagger/OpenAPI

## License

This project is licensed under the MIT License - see the LICENSE file for details.
