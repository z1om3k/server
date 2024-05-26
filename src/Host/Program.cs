using Core;
using Core.Services;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
    });
}

app.UseCors();
app.MapGet("/", () => "Hello World!");

app.MapGet("/github/{endpoint}", async (string endpoint, [FromServices] IApiService apiService) =>
{
    return await apiService.FetchFromGitHubAsync(endpoint);
});

app.MapGet("/spotify/{endpoint}", async (string endpoint, [FromServices] IApiService apiService) =>
{
    return await apiService.FetchFromSpotifyAsync(endpoint);
});

app.Run();
