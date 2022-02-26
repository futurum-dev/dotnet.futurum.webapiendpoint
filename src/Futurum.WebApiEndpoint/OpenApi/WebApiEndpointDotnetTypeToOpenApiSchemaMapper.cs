using Microsoft.OpenApi.Models;

namespace Futurum.WebApiEndpoint.OpenApi;

/// <summary>
/// Maps dotnet types to OpenApiSchema
/// https://swagger.io/specification/
/// </summary>
internal static class WebApiEndpointDotnetTypeToOpenApiSchemaMapper
{
    public static OpenApiSchema Execute(Type type)
    {
        if (type == typeof(string))
        {
            return new OpenApiSchema { Type = "string" };
        }

        if (type == typeof(int))
        {
            return new OpenApiSchema { Type = "integer", Format = "int32" };
        }

        if (type == typeof(long))
        {
            return new OpenApiSchema { Type = "integer", Format = "int64" };
        }

        if (type == typeof(float))
        {
            return new OpenApiSchema { Type = "number", Format = "float" };
        }

        if (type == typeof(double))
        {
            return new OpenApiSchema { Type = "number", Format = "double" };
        }

        if (type == typeof(byte))
        {
            return new OpenApiSchema { Type = "string", Format = "byte" };
        }

        if (type == typeof(DateTime))
        {
            return new OpenApiSchema { Type = "string", Format = "date-time" };
        }

        if (type == typeof(bool))
        {
            return new OpenApiSchema { Type = "boolean" };
        }

        if (type == typeof(Guid))
        {
            return new OpenApiSchema { Type = "string", Format = "uuid" };
        }

        if (type == typeof(IFormFile))
        {
            return new OpenApiSchema { Type = "string", Format = "binary" };
        }

        if (type == typeof(IEnumerable<IFormFile>))
        {
            return new OpenApiSchema { Type = "array", Items = new OpenApiSchema { Type = "string", Format = "binary" } };
        }

        if (type.IsClass && !type.IsPrimitive)
        {
            return new OpenApiSchema
            {
                Type = "object",
                Properties = type.GetProperties()
                                 .Select(x => (x.Name, propertyOpenApiSchema: Execute(x.PropertyType)))
                                 .ToDictionary(x => x.Name, x => x.propertyOpenApiSchema)
            };
        }

        return null;
    }
}