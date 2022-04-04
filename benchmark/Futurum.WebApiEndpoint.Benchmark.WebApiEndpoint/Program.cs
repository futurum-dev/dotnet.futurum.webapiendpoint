using Futurum.ApiEndpoint;
using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint;
using Futurum.WebApiEndpoint.Benchmark.WebApiEndpoint;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

builder.Host.ConfigureServices(serviceCollection =>
{
    serviceCollection.RegisterModule(new WebApiEndpointModule(typeof(AssemblyHook).Assembly));

    serviceCollection.AddSingleton<IWebApiEndpointLogger, NoOpWebApiEndpointLogger>();
    serviceCollection.AddSingleton<IApiEndpointLogger, NoOpWebApiEndpointLogger>();
});

// builder.Services.EnableOpenApiForWebApiEndpoint();
//
// builder.Services.AddOpenApiVersion("WebApiEndpoint Benchmark", WebApiEndpointVersions.V1_0);

builder.Services.AddAuthorization();

builder.Services.Configure<JsonOptions>(options => { options.SerializerOptions.AddContext<WebApiEndpointJsonSerializerContext>(); });

var application = builder.Build();

application.UseAuthorization();

// if (application.Environment.IsDevelopment())
// {
//     application.UseOpenApiUIForWebApiEndpoint();
// }

application.UseWebApiEndpoints();

application.Run();

namespace Futurum.WebApiEndpoint.Benchmark.WebApiEndpoint
{
    public class Program
    {
    }
}