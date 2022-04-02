using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request with MapFromMultipart
    /// </summary>
    public abstract class RequestMapFromMultipart<TRequestDto, TRequest> 
        where TRequestDto : class, new()
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class NoResponse
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<TRequestDto, ResponseEmptyDto, TRequest, ResponseEmpty, RequestMapFromMultipartMapper<TRequestDto, TRequest, TRequestPayloadMapper>, ResponseEmptyMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseEmpty>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<TRequestDto, ResponseJsonDto<TResponseDto>, TRequest, TResponseDomain, RequestMapFromMultipartMapper<TRequestDto, TRequest, TMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>, IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestPayloadMapper, TResponseMapper> : ICommandWebApiEndpoint<TRequestDto, ResponseJsonDto<TResponseDto>, TRequest, TResponseDomain, RequestMapFromMultipartMapper<TRequestDto, TRequest, TRequestPayloadMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TResponseMapper>>
                where TRequestPayloadMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>
                where TResponseMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TRequestPayloadMapper, TResponseDataMapper> : ICommandWebApiEndpoint<TRequestDto, ResponseAsyncEnumerableDto<TDataDto>, TRequest, ResponseAsyncEnumerable<TData>, RequestMapFromMultipartMapper<TRequestDto, TRequest, TRequestPayloadMapper>, ResponseAsyncEnumerableMapper<TData, TDataDto, TResponseDataMapper>>
                where TRequestPayloadMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<TRequestDto, ResponseBytesDto, TRequest, WebApiEndpoint.ResponseBytes, RequestMapFromMultipartMapper<TRequestDto, TRequest, TRequestPayloadMapper>, ResponseBytesMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<TRequestDto, ResponseDataCollectionDto<TDataDto>, TRequest, ResponseDataCollection<TData>, RequestMapFromMultipartMapper<TRequestDto, TRequest, TMapper>, ResponseDataCollectionMapper<TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestPayloadMapper, TResponseDataMapper> : ICommandWebApiEndpoint<TRequestDto, ResponseDataCollectionDto<TDataDto>, TRequest, ResponseDataCollection<TData>, RequestMapFromMultipartMapper<TRequestDto, TRequest, TRequestPayloadMapper>, ResponseDataCollectionMapper<TData, TDataDto, TResponseDataMapper>>
                where TRequestPayloadMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<TRequestDto, ResponseEmptyJsonDto, TRequest, WebApiEndpoint.ResponseEmptyJson, RequestMapFromMultipartMapper<TRequestDto, TRequest, TRequestPayloadMapper>, ResponseEmptyJsonMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<TRequestDto, ResponseFileStreamDto, TRequest, WebApiEndpoint.ResponseFileStream, RequestMapFromMultipartMapper<TRequestDto, TRequest, TRequestPayloadMapper>, ResponseFileStreamMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<TRequestDto, ResponseStreamDto, TRequest, WebApiEndpoint.ResponseStream, RequestMapFromMultipartMapper<TRequestDto, TRequest, TRequestPayloadMapper>, ResponseStreamMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response redirect
        /// </summary>
        public abstract class ResponseRedirect
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<TRequestDto, ResponseRedirectDto, TRequest, WebApiEndpoint.ResponseRedirect, RequestMapFromMultipartMapper<TRequestDto, TRequest, TRequestPayloadMapper>, ResponseRedirectMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestMapper<TRequestDto, TRequest>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseRedirect>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
            }
        }
    }
}