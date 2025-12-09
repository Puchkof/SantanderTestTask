using HackerNews.API.Endpoints;
using HackerNews.API.Extensions;
using HackerNews.Application;
using HackerNews.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapStoriesEndpoints();

app.Run();

public partial class Program { }
