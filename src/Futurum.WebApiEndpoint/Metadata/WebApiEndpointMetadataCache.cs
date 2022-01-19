using System.Reflection;

using Futurum.ApiEndpoint.DebugLogger;
using Futurum.Core.Option;
using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Metadata;

internal interface IWebApiEndpointMetadataCache
{
    IEnumerable<KeyValuePair<WebApiEndpointMetadataCacheKey, MetadataDefinition>> GetAll();
    Result<MetadataDefinition> Get(WebApiEndpointMetadataCacheKey key);
}

internal record struct WebApiEndpointMetadataCacheKey(string HttpMethod, string Route);

internal class WebApiEndpointMetadataCache : IWebApiEndpointMetadataCache
{
    private readonly WebApiEndpointConfiguration _configuration;

    private readonly Dictionary<WebApiEndpointMetadataCacheKey, MetadataDefinition> _cache = new();

    public WebApiEndpointMetadataCache(IApiEndpointDebugLogger apiEndpointDebugLogger,
                                       IWebApiEndpointAssemblyStore assemblyStore,
                                       WebApiEndpointConfiguration configuration)
    {
        _configuration = configuration;

        UpdateCache(assemblyStore.Get());

        LogApiEndpointDefinitionsDebug(apiEndpointDebugLogger, assemblyStore.Get());
    }

    private void UpdateCache(IEnumerable<Assembly> assemblies)
    {
        foreach (var apiEndpointDefinition in WebApiEndpointOnApiEndpointDefinitionMetadataProvider.GetMetadata(assemblies))
        {
            var key = GetCacheKey(apiEndpointDefinition);

            if (!_cache.ContainsKey(key))
            {
                _cache.Add(key, apiEndpointDefinition);
            }
        }
    }

    private static void LogApiEndpointDefinitionsDebug(IApiEndpointDebugLogger apiEndpointDebugLogger, IEnumerable<Assembly> assemblies)
    {
        var apiEndpointDebugNodes = WebApiEndpointOnApiEndpointDefinitionMetadataProvider.GetDebug(assemblies);
        apiEndpointDebugLogger.Execute(apiEndpointDebugNodes.ToList());
    }

    private WebApiEndpointMetadataCacheKey GetCacheKey(MetadataDefinition metadataDefinition)
    {
        var route = _configuration.RouteFactory(_configuration, metadataDefinition.MetadataRouteDefinition);

        return new WebApiEndpointMetadataCacheKey(metadataDefinition.MetadataRouteDefinition.HttpMethod.ToString().ToUpper(), route);
    }

    public IEnumerable<KeyValuePair<WebApiEndpointMetadataCacheKey, MetadataDefinition>> GetAll() => _cache;

    public Result<MetadataDefinition> Get(WebApiEndpointMetadataCacheKey key) =>
        _cache.TryGetValue(key, () => $"Unable to find WebApiEndpoint Metadata for Key '{key}'");
}