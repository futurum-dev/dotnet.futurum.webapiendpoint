using System.Reflection;

namespace Futurum.WebApiEndpoint.Metadata;

/// <summary>
/// Metadata definition for a WebApiEndpoint MapFromMultipart Parameter
/// </summary>
public record MetadataMapFromMultipartParameterDefinition(string Name, PropertyInfo PropertyInfo, MapFromMultipartAttribute MapFromMultipartAttribute);