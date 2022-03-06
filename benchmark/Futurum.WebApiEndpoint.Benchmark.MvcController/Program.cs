using FluentValidation;

using Futurum.WebApiEndpoint.Benchmark.MvcController;

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

    builder.Host.ConfigureServices(serviceCollection => serviceCollection.AddSingleton<IValidator<RequestDto>, Validator>());


// Add services to the container.

    builder.Services.AddControllers()
           .AddJsonOptions(options =>
           {
               options.JsonSerializerOptions.AddContext<WebApiEndpointJsonSerializerContext>();
           });
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

    builder.Services.AddAuthorization();

    var application = builder.Build();

    application.UseAuthorization();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

//
// application.UseAuthorization();

    application.MapControllers();

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

namespace Futurum.WebApiEndpoint.Benchmark.MvcController
{
    public partial class Program
    {
    }
}