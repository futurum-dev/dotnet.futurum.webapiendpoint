using System.Text.Json;

using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint;
using Futurum.WebApiEndpoint.Benchmark.WebApiEndpoint;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.AspNetCore.Http.Json;

using Serilog;

Log.Logger = new LoggerConfiguration()
             .Enrich.FromLogContext()
             .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
             .CreateBootstrapLogger();

try
{
    Log.Information("Application starting up");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
                                loggerConfiguration.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                                                   .ReadFrom.Configuration(hostBuilderContext.Configuration));
    
    builder.Host.ConfigureServices(serviceCollection => serviceCollection.RegisterModule(new WebApiEndpointModule(typeof(AssemblyHook).Assembly)));

    // builder.Services.EnableOpenApiForWebApiEndpoint();
    //
    // builder.Services.AddOpenApiVersion("WebApiEndpoint Benchmark", WebApiEndpointVersions.V1_0);

    builder.Services.AddAuthorization();

    builder.Services.Configure<JsonOptions>(options =>
    {
        options.SerializerOptions.AddContext<WebApiEndpointJsonSerializerContext>();
    });
    
    var application = builder.Build();
    
    application.UseAuthorization();

    // if (application.Environment.IsDevelopment())
    // {
    //     application.UseOpenApiUIForWebApiEndpoint();
    // }

    application.UseWebApiEndpoints();

    application.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Application start-up failed");
}
finally
{
    Log.Information("Application shut down complete");
    Log.CloseAndFlush();
}

namespace Futurum.WebApiEndpoint.Benchmark.WebApiEndpoint
{
    public partial class Program
    {
    }
}