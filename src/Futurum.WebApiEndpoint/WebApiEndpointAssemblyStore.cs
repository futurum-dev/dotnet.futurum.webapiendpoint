using System.Reflection;

namespace Futurum.WebApiEndpoint;

public interface IWebApiEndpointAssemblyStore
{
    IEnumerable<Assembly> Get();
}

public class WebApiEndpointAssemblyStore : IWebApiEndpointAssemblyStore
{
    private readonly IEnumerable<Assembly> _assemblies;

    public WebApiEndpointAssemblyStore(IEnumerable<Assembly>assemblies)
    {
        _assemblies = assemblies;
    }
    
    public IEnumerable<Assembly> Get() =>
        _assemblies;
}