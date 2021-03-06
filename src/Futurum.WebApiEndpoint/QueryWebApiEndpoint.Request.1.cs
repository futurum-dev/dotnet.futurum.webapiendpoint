using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class QueryWebApiEndpoint
{
    /// <summary>
    /// Configure with request
    /// </summary>
    public abstract class Request<TQueryDomain>
    {
        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseJsonDto<TResponseDto>, TQueryDomain, TResponseDomain, 
                RequestEmptyMapper<TQueryDomain, TMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TQueryDomain>, IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
            
            public abstract class Mapper<TRequestMapper, TResponseMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseJsonDto<TResponseDto>, TQueryDomain, TResponseDomain, 
                RequestEmptyMapper<TQueryDomain, TRequestMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TResponseMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
                where TResponseMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseAsyncEnumerableDto<TDataDto>, TQueryDomain, ResponseAsyncEnumerable<TData>, 
                RequestEmptyMapper<TQueryDomain, TMapper>, ResponseAsyncEnumerableMapper<TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TQueryDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
            
            public abstract class Mapper<TRequestMapper, TResponseDataMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseAsyncEnumerableDto<TDataDto>, TQueryDomain, ResponseAsyncEnumerable<TData>, 
                RequestEmptyMapper<TQueryDomain, TRequestMapper>, ResponseAsyncEnumerableMapper<TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes
        {
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseBytesDto, TQueryDomain, WebApiEndpoint.ResponseBytes, 
                RequestEmptyMapper<TQueryDomain, TRequestMapper>, ResponseBytesMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseDataCollectionDto<TDataDto>, TQueryDomain, ResponseDataCollection<TData>, 
                RequestEmptyMapper<TQueryDomain, TMapper>, ResponseDataCollectionMapper<TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TQueryDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
            
            public abstract class Mapper<TRequestMapper, TResponseDataMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseDataCollectionDto<TDataDto>, TQueryDomain, ResponseDataCollection<TData>, 
                RequestEmptyMapper<TQueryDomain, TRequestMapper>, ResponseDataCollectionMapper<TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson
        {
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseEmptyJsonDto, TQueryDomain, WebApiEndpoint.ResponseEmptyJson, 
                RequestEmptyMapper<TQueryDomain, TRequestMapper>, ResponseEmptyJsonMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream
        {
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseFileStreamDto, TQueryDomain, WebApiEndpoint.ResponseFileStream, 
                RequestEmptyMapper<TQueryDomain, TRequestMapper>, ResponseFileStreamMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream
        {
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseStreamDto, TQueryDomain, WebApiEndpoint.ResponseStream,
                RequestEmptyMapper<TQueryDomain, TRequestMapper>, ResponseStreamMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response redirect
        /// </summary>
        public abstract class ResponseRedirect
        {
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseRedirectDto, TQueryDomain, WebApiEndpoint.ResponseRedirect,
                RequestEmptyMapper<TQueryDomain, TRequestMapper>, ResponseRedirectMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseRedirect>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
        }
    }
}