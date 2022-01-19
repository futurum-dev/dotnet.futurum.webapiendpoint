using Extender;

using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

internal interface IRequestOpenApiTypeCreator
{
    Type Create(MetadataMapFromDefinition metadataMapFromDefinition, Type requestDtoType);

    Type Get(Type originalRequestDtoType);
}

internal class RequestOpenApiTypeCreator : IRequestOpenApiTypeCreator
{
    private readonly Dictionary<Type, Type> _cache = new();

    public Type Create(MetadataMapFromDefinition metadataMapFromDefinition, Type requestDtoType)
    {
        if (_cache.ContainsKey(requestDtoType))
        {
            return _cache[requestDtoType];
        }
        
        var requestDtoOpenApiType = CreateRequestOpenApiType(metadataMapFromDefinition, requestDtoType);
        _cache.Add(requestDtoType, requestDtoOpenApiType);

        return requestDtoOpenApiType;
    }

    public Type Get(Type originalRequestDtoType) =>
        _cache[originalRequestDtoType];

    private static Type CreateRequestOpenApiType(MetadataMapFromDefinition metadataMapFromDefinition, Type requestDtoType)
    {
        var typeExtender = new TypeExtender(requestDtoType.FullName, typeof(object));
        
        foreach (var propertyInfo in requestDtoType.GetProperties())
        {
            if (!metadataMapFromDefinition.MapFromParameterDefinitions.Select(x => x.PropertyInfo).Contains(propertyInfo))
            {
                typeExtender.AddProperty(propertyInfo.Name, propertyInfo.PropertyType);
            }
        }

        return typeExtender.FetchType();
    }
}