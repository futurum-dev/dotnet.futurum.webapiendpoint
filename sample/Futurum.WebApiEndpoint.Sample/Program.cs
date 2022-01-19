using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint;
using Futurum.WebApiEndpoint.OpenApi;
using Futurum.WebApiEndpoint.Sample;

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

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options => options.EnableWebApiEndpointForOpenApi("WebApiEndpoint Samples", WebApiEndpointVersions.V1_0, WebApiEndpointVersions.V2_0)
                                                     .EnableWebApiEndpointJwtForOpenApi());
    
    builder.Services.AddWebApiEndpointAuthorization(typeof(AssemblyHook).Assembly);
    builder.Services.AddAuthenticationJwtBearer(builder.Configuration["Jwt:Issuer"], builder.Configuration["Jwt:Audience"], builder.Configuration["Jwt:Key"]);

    var application = builder.Build();
    
    application.UseAuthentication();
    application.UseAuthorization();

    if (application.Environment.IsDevelopment())
    {
        application.UseSwagger();
        application.UseSwaggerUI(options => options.UseWebApiEndpointOpenApiUI("WebApiEndpoint Samples", WebApiEndpointVersions.V1_0, WebApiEndpointVersions.V2_0));
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