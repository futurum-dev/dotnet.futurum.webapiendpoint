using Futurum.ApiEndpoint;

namespace Futurum.WebApiEndpoint.Sample.Features;

public class ApiEndpointDefinition : IApiEndpointDefinition
{
    public void Configure(ApiEndpointDefinitionBuilder definitionBuilder)
    {
        definitionBuilder.Web()
                         
                         .Query<QueryWithoutRequestWithResponseScenario.ApiEndpoint>(builder => builder.Route("query-without-request-with-response"))
                         .Query<QueryWithoutRequestWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Route("query-without-request-with-response-async-enumerable"))
                         .Query<QueryWithoutRequestWithResponseBytesScenario.ApiEndpoint>(builder => builder.Route("query-without-request-with-response-bytes"))
                         .Query<QueryWithoutRequestWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Route("query-without-request-with-data-collection"))
                         .Query<QueryWithoutRequestWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Route("query-without-request-with-response-empty-json"))
                         .Query<QueryWithoutRequestWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Route("query-without-request-with-response-file-stream"))
                         .Query<QueryWithoutRequestWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Route("query-without-request-with-response-file-stream-with-content-type"))
                         .Query<QueryWithoutRequestWithResponseStreamScenario.ApiEndpoint>(builder => builder.Route("query-without-request-with-response-stream"))
                         
                         .Query<QueryWithRequestParameterMapFromWithResponseScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterMapFromWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response-async-enumerable/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterMapFromWithResponseBytesScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response-bytes/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterMapFromWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-data-collection/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterMapFromWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response-empty-json/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterMapFromWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response-file-stream/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterMapFromWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response-file-stream-with-content-type/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterMapFromWithResponseStreamScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response-stream/{Id}", ("Id", typeof(string))))
                         
                         .Query<QueryWithRequestParameterWithResponseScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-with-response/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-with-response-async-enumerable/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterWithResponseBytesScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-with-response-bytes/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-with-data-collection/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-with-response-empty-json/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-with-response-file-stream/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-with-response-file-stream-with-content-type/{Id}", ("Id", typeof(string))))
                         .Query<QueryWithRequestParameterWithResponseStreamScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-with-response-stream/{Id}", ("Id", typeof(string))))
                         
                         .Command<CommandWithRequestParameterMapFromWithResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-with-request/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterMapFromWithoutResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-without-response/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterMapFromWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-with-response-async-enumerable/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterMapFromWithResponseBytesScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-with-response-bytes/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterMapFromWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-with-response-data-collection/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterMapFromWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-with-response-empty-json/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterMapFromWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-with-response-file-stream/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterWithResponseMapFromFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-with-response-file-stream-with-content-type/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterMapFromWithResponseStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-with-response-stream/{Id}", ("Id", typeof(string))))
                         
                         .Command<CommandWithRequestParameterWithResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterWithoutResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-without-response/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-async-enumerable/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterWithResponseBytesScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-bytes/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-data-collection/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-empty-json/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-file-stream/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-file-stream-with-content-type/{Id}", ("Id", typeof(string))))
                         .Command<CommandWithRequestParameterWithResponseStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-stream/{Id}", ("Id", typeof(string))))
                         
                         .Command<CommandWithRequestPlainTextWithResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response"))
                         .Command<CommandWithRequestPlainTextWithoutResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-without-response"))
                         .Command<CommandWithRequestPlainTextWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-async-enumerable"))
                         .Command<CommandWithRequestPlainTextWithResponseBytesScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-bytes"))
                         .Command<CommandWithRequestPlainTextWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-data-collection"))
                         .Command<CommandWithRequestPlainTextWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-empty-json"))
                         .Command<CommandWithRequestPlainTextWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-file-stream"))
                         .Command<CommandWithRequestPlainTextWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-file-stream-with-content-type"))
                         .Command<CommandWithRequestPlainTextWithResponseStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-stream"))
                         
                         .Command<CommandWithRequestUploadFilesWithResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response"))
                         .Command<CommandWithRequestUploadFilesWithoutResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-without-response"))
                         .Command<CommandWithRequestUploadFilesWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-async-enumerable"))
                         .Command<CommandWithRequestUploadFilesWithResponseBytesScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-bytes"))
                         .Command<CommandWithRequestUploadFilesWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-data-collection"))
                         .Command<CommandWithRequestUploadFilesWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-empty-json"))
                         .Command<CommandWithRequestUploadFilesWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-file-stream"))
                         .Command<CommandWithRequestUploadFilesWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-file-stream-with-content-type"))
                         .Command<CommandWithRequestUploadFilesWithResponseStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-stream"))
                         
                         .Command<CommandWithRequestWithResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-with-response"))
                         .Command<CommandWithRequestWithoutResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-without-response"))
                         .Command<CommandWithRequestWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Post("command-with-request-with-response-async-enumerable"))
                         .Command<CommandWithRequestWithResponseBytesScenario.ApiEndpoint>(builder => builder.Post("command-with-request-with-response-bytes"))
                         .Command<CommandWithRequestWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Post("command-with-request-with-response-data-collection"))
                         .Command<CommandWithRequestWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Post("command-with-request-with-response-empty-json"))
                         .Command<CommandWithRequestWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-with-response-file-stream"))
                         .Command<CommandWithRequestWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Post("command-with-request-with-response-file-stream-with-content-type"))
                         .Command<CommandWithRequestWithResponseStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-with-response-stream"))
                         ;
    }
}