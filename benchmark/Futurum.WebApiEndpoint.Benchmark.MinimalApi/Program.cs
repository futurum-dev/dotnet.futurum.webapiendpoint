using FluentValidation;

using Futurum.WebApiEndpoint.Benchmark.MinimalApi;

using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

builder.Host.ConfigureServices(serviceCollection => serviceCollection.AddSingleton<IValidator<TestEndpoint.RequestDto>, TestEndpoint.Validator>());

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

builder.Services.Configure<JsonOptions>(options => { options.SerializerOptions.AddContext<WebApiEndpointJsonSerializerContext>(); });

var application = builder.Build();

application.UseAuthorization();

// if (application.Environment.IsDevelopment())
// {
//     application.UseSwagger();
//     application.UseSwaggerUI();
// }

application.MapPost("api/benchmark/{id}", TestEndpoint.Execute)
           .AllowAnonymous();

application.Run();

namespace Futurum.WebApiEndpoint.Benchmark.MinimalApi
{
    public partial class Program
    {
    }
}