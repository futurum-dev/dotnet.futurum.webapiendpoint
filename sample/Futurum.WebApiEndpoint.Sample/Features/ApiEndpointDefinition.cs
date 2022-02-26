using Futurum.ApiEndpoint;
using Futurum.WebApiEndpoint.Sample.Features.CommandWithRequest;
using Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestManualParameter;
using Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestParameterMapFrom;
using Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;
using Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;
using Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFiles;
using Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;
using Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;
using Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestManualParameter;
using Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestParameterMapFrom;

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
                         
                         .Query<QueryWithRequestParameterMapFromWithResponseScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response/{Id}"))
                         .Query<QueryWithRequestParameterMapFromWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response-async-enumerable/{Id}"))
                         .Query<QueryWithRequestParameterMapFromWithResponseBytesScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response-bytes/{Id}"))
                         .Query<QueryWithRequestParameterMapFromWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-data-collection/{Id}"))
                         .Query<QueryWithRequestParameterMapFromWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response-empty-json/{Id}"))
                         .Query<QueryWithRequestParameterMapFromWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response-file-stream/{Id}"))
                         .Query<QueryWithRequestParameterMapFromWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response-file-stream-with-content-type/{Id}"))
                         .Query<QueryWithRequestParameterMapFromWithResponseStreamScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-with-response-stream/{Id}"))
                         
                         .Query<QueryWithRequestParameterMapFromSupportedTypesScenario.ApiEndpoint>(builder => builder.Route("query-with-request-parameter-map-from-supported-types/{String}/{Int}/{Long}/{DateTime}/{Boolean}/{Guid}"))
                         
                         .Query<QueryWithRequestManualParameterWithResponseScenario.ApiEndpoint>(builder => builder.Route("query-with-request-manual-parameter-with-response/{Id}"))
                         .Query<QueryWithRequestManualParameterWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Route("query-with-request-manual-parameter-with-response-async-enumerable/{Id}"))
                         .Query<QueryWithRequestManualParameterWithResponseBytesScenario.ApiEndpoint>(builder => builder.Route("query-with-request-manual-parameter-with-response-bytes/{Id}"))
                         .Query<QueryWithRequestManualParameterWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Route("query-with-request-manual-parameter-with-data-collection/{Id}"))
                         .Query<QueryWithRequestManualParameterWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Route("query-with-request-manual-parameter-with-response-empty-json/{Id}"))
                         .Query<QueryWithRequestManualParameterWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Route("query-with-request-manual-parameter-with-response-file-stream/{Id}"))
                         .Query<QueryWithRequestManualParameterWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Route("query-with-request-manual-parameter-with-response-file-stream-with-content-type/{Id}"))
                         .Query<QueryWithRequestManualParameterWithResponseStreamScenario.ApiEndpoint>(builder => builder.Route("query-with-request-manual-parameter-with-response-stream/{Id}"))
                         
                         .Command<CommandWithRequestParameterMapFromWithResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response/{Id}"))
                         .Command<CommandWithRequestParameterMapFromWithoutResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-without-response/{Id}"))
                         .Command<CommandWithRequestParameterMapFromWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-async-enumerable/{Id}"))
                         .Command<CommandWithRequestParameterMapFromWithResponseBytesScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-bytes/{Id}"))
                         .Command<CommandWithRequestParameterMapFromWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-data-collection/{Id}"))
                         .Command<CommandWithRequestParameterMapFromWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-empty-json/{Id}"))
                         .Command<CommandWithRequestParameterMapFromWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-file-stream/{Id}"))
                         .Command<CommandWithRequestParameterMapFromWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-file-stream-with-content-type/{Id}"))
                         .Command<CommandWithRequestParameterMapFromWithResponseStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-with-response-stream/{Id}"))
                         
                         .Command<CommandWithRequestParameterMapFromSupportedTypesScenario.ApiEndpoint>(builder => builder.Post("command-with-request-parameter-map-from-supported-types/{String}/{Int}/{Long}/{DateTime}/{Boolean}/{Guid}"))
                         
                         .Command<CommandWithRequestManualParameterWithResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-manual-parameter-with-response/{Id}"))
                         .Command<CommandWithRequestManualParameterWithoutResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-manual-parameter-without-response/{Id}"))
                         .Command<CommandWithRequestManualParameterWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Post("command-with-request-manual-parameter-with-response-async-enumerable/{Id}"))
                         .Command<CommandWithRequestManualParameterWithResponseBytesScenario.ApiEndpoint>(builder => builder.Post("command-with-request-manual-parameter-with-response-bytes/{Id}"))
                         .Command<CommandWithRequestManualParameterWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Post("command-with-request-manual-parameter-with-response-data-collection/{Id}"))
                         .Command<CommandWithRequestManualParameterWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Post("command-with-request-manual-parameter-with-response-empty-json/{Id}"))
                         .Command<CommandWithRequestManualParameterWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-manual-parameter-with-response-file-stream/{Id}"))
                         .Command<CommandWithRequestManualParameterWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Post("command-with-request-manual-parameter-with-response-file-stream-with-content-type/{Id}"))
                         .Command<CommandWithRequestManualParameterWithResponseStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-manual-parameter-with-response-stream/{Id}"))
                         
                         .Command<CommandWithRequestPlainTextWithResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response"))
                         .Command<CommandWithRequestPlainTextWithoutResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-without-response"))
                         .Command<CommandWithRequestPlainTextWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-async-enumerable"))
                         .Command<CommandWithRequestPlainTextWithResponseBytesScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-bytes"))
                         .Command<CommandWithRequestPlainTextWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-data-collection"))
                         .Command<CommandWithRequestPlainTextWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-empty-json"))
                         .Command<CommandWithRequestPlainTextWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-file-stream"))
                         .Command<CommandWithRequestPlainTextWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-file-stream-with-content-type"))
                         .Command<CommandWithRequestPlainTextWithResponseStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-plain-text-with-response-stream"))
                         
                         .Command<CommandWithRequestUploadFileWithPayloadWithResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-file-and-json-with-response"))
                         .Command<CommandWithRequestUploadFileWithPayloadSupportedTypesScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-file-and-json-supported-types"))
                         
                         .Command<CommandWithRequestUploadFilesWithResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response"))
                         .Command<CommandWithRequestUploadFilesWithoutResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-without-response"))
                         .Command<CommandWithRequestUploadFilesWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-async-enumerable"))
                         .Command<CommandWithRequestUploadFilesWithResponseBytesScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-bytes"))
                         .Command<CommandWithRequestUploadFilesWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-data-collection"))
                         .Command<CommandWithRequestUploadFilesWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-empty-json"))
                         .Command<CommandWithRequestUploadFilesWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-file-stream"))
                         .Command<CommandWithRequestUploadFilesWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-file-stream-with-content-type"))
                         .Command<CommandWithRequestUploadFilesWithResponseStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-files-with-response-stream"))
                         
                         .Command<CommandWithRequestUploadFileWithResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-file-with-response"))
                         .Command<CommandWithRequestUploadFileWithoutResponseScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-file-without-response"))
                         .Command<CommandWithRequestUploadFileWithResponseAsyncEnumerableScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-file-with-response-async-enumerable"))
                         .Command<CommandWithRequestUploadFileWithResponseBytesScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-file-with-response-bytes"))
                         .Command<CommandWithRequestUploadFileWithResponseDataCollectionScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-file-with-response-data-collection"))
                         .Command<CommandWithRequestUploadFileWithResponseEmptyJsonScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-file-with-response-empty-json"))
                         .Command<CommandWithRequestUploadFileWithResponseFileStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-file-with-response-file-stream"))
                         .Command<CommandWithRequestUploadFileWithResponseFileStreamWithContentTypeScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-file-with-response-file-stream-with-content-type"))
                         .Command<CommandWithRequestUploadFileWithResponseStreamScenario.ApiEndpoint>(builder => builder.Post("command-with-request-upload-file-with-response-stream"))
                         
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