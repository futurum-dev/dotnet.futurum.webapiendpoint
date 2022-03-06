using FluentValidation;

using Futurum.WebApiEndpoint.Benchmark.MinimalApi;

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

    builder.Host.ConfigureServices(serviceCollection => serviceCollection.AddSingleton<IValidator<TestEndpoint.RequestDto>, TestEndpoint.Validator>());

    // builder.Services.AddEndpointsApiExplorer();
    // builder.Services.AddSwaggerGen();

    builder.Services.AddAuthorization();

    builder.Services.Configure<JsonOptions>(options =>
    {
        options.SerializerOptions.AddContext<WebApiEndpointJsonSerializerContext>();
    });
    
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

namespace Futurum.WebApiEndpoint.Benchmark.MinimalApi
{
    public partial class Program
    {
    }
}