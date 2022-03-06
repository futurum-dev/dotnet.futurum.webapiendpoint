using FluentValidation;

using Futurum.WebApiEndpoint.Benchmark.MvcController;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

builder.Host.ConfigureServices(serviceCollection => serviceCollection.AddSingleton<IValidator<RequestDto>, Validator>());

// Add services to the container.

builder.Services.AddControllers()
       .AddJsonOptions(options => { options.JsonSerializerOptions.AddContext<WebApiEndpointJsonSerializerContext>(); });

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

application.UseAuthorization();

application.MapControllers();

application.Run();

namespace Futurum.WebApiEndpoint.Benchmark.MvcController
{
    public partial class Program
    {
    }
}