using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Builder for <see cref="IQueryWebApiEndpoint"/>
/// </summary>
public abstract partial class QueryWebApiEndpoint
{
    /// <summary>
    /// Configure without request
    /// </summary>
    public abstract class NoRequest
    {
        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseJsonDto<TResponseDto>, RequestEmpty, TResponseDomain, 
                RequestEmptyMapper, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseAsyncEnumerableDto<TDataDto>, RequestEmpty, ResponseAsyncEnumerable<TData>, 
                RequestEmptyMapper, ResponseAsyncEnumerableMapper<TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes : IQueryWebApiEndpoint<RequestEmptyDto, ResponseBytesDto, RequestEmpty, WebApiEndpoint.ResponseBytes, RequestEmptyMapper, ResponseBytesMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseDataCollectionDto<TDataDto>, RequestEmpty, ResponseDataCollection<TData>, 
                RequestEmptyMapper, ResponseDataCollectionMapper<TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson : IQueryWebApiEndpoint<RequestEmptyDto, ResponseEmptyJsonDto, RequestEmpty, WebApiEndpoint.ResponseEmptyJson, 
            RequestEmptyMapper, ResponseEmptyJsonMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream : IQueryWebApiEndpoint<RequestEmptyDto, ResponseFileStreamDto, RequestEmpty, WebApiEndpoint.ResponseFileStream, 
            RequestEmptyMapper, ResponseFileStreamMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream : IQueryWebApiEndpoint<RequestEmptyDto, ResponseStreamDto, RequestEmpty, WebApiEndpoint.ResponseStream, 
            RequestEmptyMapper, ResponseStreamMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response redirect
        /// </summary>
        public abstract class ResponseRedirect : IQueryWebApiEndpoint<RequestEmptyDto, ResponseRedirectDto, RequestEmpty, WebApiEndpoint.ResponseRedirect, 
            RequestEmptyMapper, ResponseRedirectMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseRedirect>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken);
        }
    }
}