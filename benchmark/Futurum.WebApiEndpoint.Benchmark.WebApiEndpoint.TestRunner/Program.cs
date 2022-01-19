using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Benchmark.WebApiEndpoint.TestRunner;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

var host = Startup(args);

try
{
    Log.Logger.Error("Starting...");

    using var serviceScope = host.Services.CreateScope();
    var provider = serviceScope.ServiceProvider;

    var app = provider.GetRequiredService<IApp>();

    await app.ExecuteAsync()
             .UnwrapAsync();

    Log.Logger.Error("Finished...");
}
catch (Exception exception)
{
    Log.Logger.Error(exception, "Error...");
}

Console.ReadKey();


static IHost Startup(string[] args)
{
    var builder = ConfigurationBuilder();

    ConfigureSerilog(builder);

    return CreateHost(args);
}

static IHost CreateHost(string[] args)
{
    return Host.CreateDefaultBuilder(args)
               .ConfigureServices((_, services) =>
               {
                   services.AddHttpClient();
                    
                   services.AddSingleton<IApp, App>();
               })
               .UseSerilog()
               .Build();
}

static ConfigurationBuilder ConfigurationBuilder()
{
    var builder = new ConfigurationBuilder();

    builder.SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables();

    return builder;
}

static void ConfigureSerilog(ConfigurationBuilder builder)
{
    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Build())
                                          .Enrich.FromLogContext()
                                          .WriteTo.Console()
                                          .CreateLogger();
}