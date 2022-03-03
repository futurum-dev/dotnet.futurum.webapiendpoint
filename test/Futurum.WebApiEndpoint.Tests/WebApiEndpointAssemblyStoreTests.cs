using FluentAssertions;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class WebApiEndpointAssemblyStoreTests
{
    [Fact]
    public void check()
    {
        var assemblies = new[]
        {
            GetType().Assembly,
            typeof(string).Assembly
        };

        var webApiEndpointAssemblyStore = new WebApiEndpointAssemblyStore(assemblies);

        var retrievedAssemblies = webApiEndpointAssemblyStore.Get();

        retrievedAssemblies.Should().BeEquivalentTo(assemblies);
    }
}