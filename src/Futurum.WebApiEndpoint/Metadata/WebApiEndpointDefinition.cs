using Futurum.ApiEndpoint;
using Futurum.ApiEndpoint.DebugLogger;
using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Metadata;

public interface IWebApiEndpointDefinition
{
    /// <summary>
    /// Configure a WebApiEndpoint Query
    /// </summary>
    IWebApiEndpointDefinition Query<TWebApiEndpoint>(Action<QueryMetadataRouteDefinitionBuilder> builderFunc)
        where TWebApiEndpoint : IQueryWebApiEndpoint;

    /// <summary>
    /// Configure a WebApiEndpoint Command
    /// </summary>
    IWebApiEndpointDefinition Command<TWebApiEndpoint>(Action<CommandMetadataRouteDefinitionBuilder> builderFunc)
        where TWebApiEndpoint : ICommandWebApiEndpoint;
}

public class WebApiEndpointDefinition : IWebApiEndpointDefinition, IApiEndpointDefinitionBuilder
{
    private readonly Dictionary<Type, List<Func<IMetadataRouteDefinitionBuilder>>> _builders = new();

    Result<Dictionary<Type, List<IMetadataDefinition>>> IApiEndpointDefinitionBuilder.Build() =>
        _builders.TryToDictionary(keyValuePair => keyValuePair.Key,
                                  keyValuePair => keyValuePair.Value.SelectMany(func => func().Build()).ToList<IMetadataDefinition>());

    ApiEndpointDebugNode IApiEndpointDefinitionBuilder.Debug()
    {
        return new ApiEndpointDebugNode
        {
            Name = "ApiEndpointDefinition",
            Children =
            {
                new ApiEndpointDebugNode
                {
                    Name = "WEB",
                    Children = _builders.SelectMany(keyValuePair => keyValuePair.Value.Select(func => func().Debug())).ToList()
                }
            }
        };
    }

    /// <inheritdoc />
    public IWebApiEndpointDefinition Query<TWebApiEndpoint>(Action<QueryMetadataRouteDefinitionBuilder> builderFunc)
        where TWebApiEndpoint : IQueryWebApiEndpoint
    {
        var builder = new QueryMetadataRouteDefinitionBuilder(typeof(TWebApiEndpoint));

        var key = typeof(TWebApiEndpoint);
        var hasRun = false;
        var value = () =>
        {
            if (!hasRun)
            {
                builderFunc(builder);

                hasRun = true;
            }

            return builder;
        };

        if (_builders.ContainsKey(key))
        {
            var existingValue = _builders[key];
            existingValue.Add(value);
        }
        else
        {
            _builders.Add(key, new List<Func<IMetadataRouteDefinitionBuilder>> { value });
        }

        return this;
    }

    /// <inheritdoc />
    public IWebApiEndpointDefinition Command<TWebApiEndpoint>(Action<CommandMetadataRouteDefinitionBuilder> builderFunc)
        where TWebApiEndpoint : ICommandWebApiEndpoint
    {
        var builder = new CommandMetadataRouteDefinitionBuilder(typeof(TWebApiEndpoint));

        var key = typeof(TWebApiEndpoint);
        var hasRun = false;
        var value = () =>
        {
            if (!hasRun)
            {
                builderFunc(builder);

                hasRun = true;
            }

            return builder;
        };

        if (_builders.ContainsKey(key))
        {
            var existingValue = _builders[key];
            existingValue.Add(value);
        }
        else
        {
            _builders.Add(key, new List<Func<IMetadataRouteDefinitionBuilder>> { value });
        }

        return this;
    }
}