using System.Text.RegularExpressions;

namespace Futurum.WebApiEndpoint.Metadata;

internal static class MetadataTypeExtensions
{
    public static string GetSanitizedLastPartOfNamespace(this Type apiEndpointType)
    {
        var lastPartOfNamespace = apiEndpointType.Namespace?.Split('.').Last();
        return Regex.Replace(lastPartOfNamespace, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}