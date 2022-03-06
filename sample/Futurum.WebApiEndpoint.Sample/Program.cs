using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint;
using Futurum.WebApiEndpoint.OpenApi;
using Futurum.WebApiEndpoint.Sample;
using Futurum.WebApiEndpoint.Sample.Features.JsonSourceGenerator;

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
    
    builder.Host.ConfigureServices(serviceCollection =>
    {
        serviceCollection.RegisterModule(new WebApiEndpointModule(typeof(AssemblyHook).Assembly));

        serviceCollection.RegisterModule<ApplicationModule>();
    });
    
    builder.Services.EnableOpenApiForWebApiEndpoint();
    builder.Services.EnableOpenApiJwtForWebApiEndpoint();

    builder.Services.AddOpenApiVersion("WebApiEndpoint Samples", WebApiEndpointVersions.V1_0);
    builder.Services.AddOpenApiVersion("WebApiEndpoint Samples", WebApiEndpointVersions.V2_0);
    builder.Services.AddOpenApiVersion("WebApiEndpoint Samples - Deprecated", WebApiEndpointVersions.V3_0, true);
    
    builder.Services.AddWebApiEndpointAuthorization(typeof(AssemblyHook).Assembly);
    builder.Services.AddAuthenticationJwtBearer(builder.Configuration["Jwt:Issuer"], builder.Configuration["Jwt:Audience"], builder.Configuration["Jwt:Key"]);

    builder.Services.Configure<JsonOptions>(options =>
    {
        options.SerializerOptions.AddContext<WebApiEndpointJsonSerializerContext>();
    });
    
    var application = builder.Build();
    
    application.UseAuthentication();
    application.UseAuthorization();

    if (application.Environment.IsDevelopment())
    {
        application.UseOpenApiUIForWebApiEndpoint();
    }

    application.UseHttpsRedirection();

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

namespace Futurum.WebApiEndpoint.Sample
{
    public partial class Program
    {
    }
}