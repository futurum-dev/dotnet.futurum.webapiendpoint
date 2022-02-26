using Futurum.Core.Functional;
using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint.OpenApi;

internal class WebApiEndpointOpenApiOperationFilter : IOperationFilter
{
    private readonly IWebApiEndpointMetadataCache _metadataCache;

    public WebApiEndpointOpenApiOperationFilter(IWebApiEndpointMetadataCache metadataCache)
    {
        _metadataCache = metadataCache;
    }

    public void Apply(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext)
    {
        if (operationFilterContext.ApiDescription.HttpMethod == MetadataRouteHttpMethod.Get.ToString() && openApiOperation.RequestBody != null)
        {
            openApiOperation.RequestBody.Required = false;
        }

        _metadataCache.Get(new WebApiEndpointMetadataCacheKey(operationFilterContext.ApiDescription.HttpMethod, operationFilterContext.ApiDescription.RelativePath))
                      .DoSwitch(metadataDefinition =>
                                {
                                    if (metadataDefinition.MetadataTypeDefinition.RequestDtoType == typeof(RequestUploadFilesDto))
                                    {
                                        ConfigureRequestUploadFilesDto(openApiOperation);
                                    }
                                    else if (metadataDefinition.MetadataTypeDefinition.RequestDtoType == typeof(RequestUploadFileDto))
                                    {
                                        ConfigureRequestUploadFileDto(openApiOperation);
                                    }
                                    else if (metadataDefinition.MetadataMapFromMultipartDefinition != null)
                                    {
                                        ConfigureMultipart(openApiOperation, metadataDefinition.MetadataMapFromMultipartDefinition);
                                    }
                                    else
                                    {
                                        ConfigureParameters(openApiOperation, metadataDefinition);
                                    }

                                    UpdateOpenApiOperationInformation(openApiOperation, metadataDefinition);
                                },
                                Function.DoNothing);
    }

    private static void ConfigureRequestUploadFilesDto(OpenApiOperation openApiOperation)
    {
        void Execute(KeyValuePair<string, OpenApiMediaType> content)
        {
            content.Value.Schema = new OpenApiSchema
            {
                Type = "object",
                Required = new HashSet<string> { nameof(RequestUploadFilesDto.Files) },
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    {
                        nameof(RequestUploadFilesDto.Files), WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(typeof(IEnumerable<IFormFile>))
                    }
                }
            };
        }

        openApiOperation.RequestBody.Content.TrySingle(x => x.Key == "multipart/form-data")
                        .Do(x => x.DoSwitch(Execute, Function.DoNothing));
    }

    private static void ConfigureRequestUploadFileDto(OpenApiOperation openApiOperation)
    {
        void Execute(KeyValuePair<string, OpenApiMediaType> content)
        {
            content.Value.Schema = new OpenApiSchema
            {
                Type = "object",
                Required = new HashSet<string> { "file" },
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    { "file", WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(typeof(IFormFile)) }
                }
            };
        }

        openApiOperation.RequestBody.Content.TrySingle(x => x.Key == "multipart/form-data")
                        .Do(x => x.DoSwitch(Execute, Function.DoNothing));
    }

    private static void ConfigureMultipart(OpenApiOperation openApiOperation, MetadataMapFromMultipartDefinition metadataMapFromMultipartDefinition)
    {
        void Execute(KeyValuePair<string, OpenApiMediaType> content)
        {
            var multipartParameterDefinitions = metadataMapFromMultipartDefinition.MapFromMultipartParameterDefinitions;
            content.Value.Schema = new OpenApiSchema
            {
                Type = "object",
                Required = new HashSet<string>(multipartParameterDefinitions.Select(x => x.Name)),
                Properties = multipartParameterDefinitions.Select(mapFromMultipartParameterDefinition =>
                                                          {
                                                              var openApiSchema = WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(mapFromMultipartParameterDefinition.PropertyInfo.PropertyType);

                                                              return (mapFromMultipartParameterDefinition.Name, OpenApiSchema: openApiSchema);
                                                          })
                                                          .ToDictionary(x => x.Name, x => x.OpenApiSchema)
            };
        }

        openApiOperation.RequestBody.Content.TrySingle(x => x.Key == "multipart/form-data")
                        .Do(x => x.DoSwitch(Execute, Function.DoNothing));
    }

    private static void ConfigureParameters(OpenApiOperation openApiOperation, MetadataDefinition metadataDefinition)
    {
        if (metadataDefinition.MetadataRouteDefinition.ManualParameterDefinitions.Any())
        {
            var openApiParameters = GetOpenApiParametersForManualParameterDefinitions(metadataDefinition.MetadataRouteDefinition.ManualParameterDefinitions);

            foreach (var openApiParameter in openApiParameters)
            {
                openApiOperation.Parameters.Add(openApiParameter);
            }
        }

        if (metadataDefinition.MetadataMapFromDefinition != null)
        {
            var openApiParameters = GetOpenApiParametersForMapFrom(metadataDefinition.MetadataMapFromDefinition);

            foreach (var openApiParameter in openApiParameters)
            {
                openApiOperation.Parameters.Add(openApiParameter);
            }
        }
    }

    private static IEnumerable<OpenApiParameter> GetOpenApiParametersForManualParameterDefinitions(IEnumerable<MetadataRouteParameterDefinition> metadataRouteParameterDefinitions)
    {
        ParameterLocation TransformMetadataRouteParameterDefinitionTypeToOpenApiParameterLocation(MetadataRouteParameterDefinitionType parameterDefinitionType) =>
            parameterDefinitionType switch
            {
                MetadataRouteParameterDefinitionType.Path   => ParameterLocation.Path,
                MetadataRouteParameterDefinitionType.Query  => ParameterLocation.Query,
                MetadataRouteParameterDefinitionType.Cookie => ParameterLocation.Cookie,
                MetadataRouteParameterDefinitionType.Header => ParameterLocation.Header,
                _                                           => throw new ArgumentOutOfRangeException(nameof(parameterDefinitionType), parameterDefinitionType, null)
            };

        return metadataRouteParameterDefinitions.Select(parameterDefinition => new OpenApiParameter
        {
            Name = parameterDefinition.Name,
            Schema = WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(parameterDefinition.Type),
            Required = true,
            In = TransformMetadataRouteParameterDefinitionTypeToOpenApiParameterLocation(parameterDefinition.ParameterDefinitionType),
        });
    }

    private static IEnumerable<OpenApiParameter> GetOpenApiParametersForMapFrom(MetadataMapFromDefinition metadataRouteParameterDefinitions)
    {
        ParameterLocation TransformMetadataRouteParameterDefinitionTypeToOpenApiParameterLocation(MapFrom mapFrom) =>
            mapFrom switch
            {
                MapFrom.Path   => ParameterLocation.Path,
                MapFrom.Query  => ParameterLocation.Query,
                MapFrom.Cookie => ParameterLocation.Cookie,
                MapFrom.Header => ParameterLocation.Header,
                _              => throw new ArgumentOutOfRangeException(nameof(mapFrom), mapFrom, null)
            };

        return metadataRouteParameterDefinitions.MapFromParameterDefinitions
                                                .Select(parameterDefinition => new OpenApiParameter
                                                {
                                                    Name = parameterDefinition.Name,
                                                    Schema = WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(parameterDefinition.PropertyInfo.PropertyType),
                                                    Required = true,
                                                    In = TransformMetadataRouteParameterDefinitionTypeToOpenApiParameterLocation(parameterDefinition.MapFromAttribute.MapFrom),
                                                });
    }

    private static void UpdateOpenApiOperationInformation(OpenApiOperation openApiOperation, MetadataDefinition metadataDefinition)
    {
        openApiOperation.Summary = metadataDefinition.MetadataRouteDefinition.OpenApiOperation?.Summary;
        openApiOperation.Description = metadataDefinition.MetadataRouteDefinition.OpenApiOperation?.Description;
    }
}