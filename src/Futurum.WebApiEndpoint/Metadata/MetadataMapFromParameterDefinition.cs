using System.Reflection;

namespace Futurum.WebApiEndpoint.Metadata;

/// <summary>
/// Metadata definition for a WebApiEndpoint MapFrom Parameter
/// </summary>
public record MetadataMapFromParameterDefinition(string Name, PropertyInfo PropertyInfo, MapFromAttribute MapFromAttribute);