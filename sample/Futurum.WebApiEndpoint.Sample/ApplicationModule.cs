using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Sample.Blog;

namespace Futurum.WebApiEndpoint.Sample;

public class ApplicationModule : IModule
{
    public void Load(IServiceCollection services)
    {
        services.AddSingleton<IBlogStorageBroker, BlogStorageBroker>();
    }
}