using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request upload file with payload
    /// </summary>
    public abstract class RequestUploadFileWithPayload<TPayloadDto, TPayload>
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class NoResponse
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseEmptyDto,
                RequestUploadFileWithPayload<TPayload>,
                ResponseEmpty, RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TRequestPayloadMapper>, ResponseEmptyMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseEmpty>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseJsonDto<TResponseDto>,
                RequestUploadFileWithPayload<TPayload>,
                TResponseDomain, RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>, IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> request, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestPayloadMapper, TResponseMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseJsonDto<TResponseDto>,
                RequestUploadFileWithPayload<TPayload>, TResponseDomain, RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TRequestPayloadMapper>,
                ResponseJsonMapper<TResponseDomain, TResponseDto, TResponseMapper>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
                where TResponseMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TRequestPayloadMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseAsyncEnumerableDto<TDataDto>,
                RequestUploadFileWithPayload<TPayload>, ResponseAsyncEnumerable<TData>, RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TRequestPayloadMapper>,
                ResponseAsyncEnumerableMapper<TData, TDataDto, TResponseDataMapper>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseBytesDto,
                RequestUploadFileWithPayload<TPayload>,
                WebApiEndpoint.ResponseBytes,
                RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TRequestPayloadMapper>, ResponseBytesMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseDataCollectionDto<TDataDto>,
                RequestUploadFileWithPayload<TPayload>, ResponseDataCollection<TData>, RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TMapper>,
                ResponseDataCollectionMapper<TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> request, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestPayloadMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseDataCollectionDto<TDataDto>,
                RequestUploadFileWithPayload<TPayload>, ResponseDataCollection<TData>, RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TRequestPayloadMapper>,
                ResponseDataCollectionMapper<TData, TDataDto, TResponseDataMapper>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseEmptyJsonDto,
                RequestUploadFileWithPayload<TPayload>,
                WebApiEndpoint.ResponseEmptyJson,
                RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TRequestPayloadMapper>, ResponseEmptyJsonMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseFileStreamDto,
                RequestUploadFileWithPayload<TPayload>,
                WebApiEndpoint.ResponseFileStream,
                RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TRequestPayloadMapper>, ResponseFileStreamMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseStreamDto,
                RequestUploadFileWithPayload<TPayload>,
                WebApiEndpoint.ResponseStream,
                RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TRequestPayloadMapper>, ResponseStreamMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response redirect
        /// </summary>
        public abstract class ResponseRedirect
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseRedirectDto,
                RequestUploadFileWithPayload<TPayload>,
                WebApiEndpoint.ResponseRedirect,
                RequestUploadFileWithPayloadMapper<TPayloadDto, TPayload, TRequestPayloadMapper>, ResponseRedirectMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseRedirect>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> request, CancellationToken cancellationToken);
            }
        }
    }
}