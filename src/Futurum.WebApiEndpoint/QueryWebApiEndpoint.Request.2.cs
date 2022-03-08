using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class QueryWebApiEndpoint
{
    /// <summary>
    /// Configure with request
    /// </summary>
    public abstract class Request<TQueryDto, TQueryDomain> 
        where TQueryDto : class
    {
        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseJsonDto<TResponseDto>, TQueryDomain, TResponseDomain, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>, IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto> 
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
            
            public abstract class Mapper<TRequestMapper, TResponseMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseJsonDto<TResponseDto>, TQueryDomain, TResponseDomain,
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TResponseMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
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
            public abstract class Mapper<TMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseAsyncEnumerableDto<TDataDto>, TQueryDomain, ResponseAsyncEnumerable<TData>, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TMapper>, ResponseAsyncEnumerableMapper<TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
            
            public abstract class Mapper<TRequestMapper, TResponseDataMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseAsyncEnumerableDto<TDataDto>, TQueryDomain, ResponseAsyncEnumerable<TData>, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseAsyncEnumerableMapper<TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
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
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseBytesDto, TQueryDomain, WebApiEndpoint.ResponseBytes, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseBytesMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
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
            public abstract class Mapper<TMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseDataCollectionDto<TDataDto>, TQueryDomain, ResponseDataCollection<TData>, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TMapper>, ResponseDataCollectionMapper<TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
            
            public abstract class Mapper<TRequestMapper, TResponseDataMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseDataCollectionDto<TDataDto>, TQueryDomain, ResponseDataCollection<TData>, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseDataCollectionMapper<TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
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
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseEmptyJsonDto, TQueryDomain, WebApiEndpoint.ResponseEmptyJson, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseEmptyJsonMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
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
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseFileStreamDto, TQueryDomain, WebApiEndpoint.ResponseFileStream, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseFileStreamMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
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
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseStreamDto, TQueryDomain, WebApiEndpoint.ResponseStream, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseStreamMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
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
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseRedirectDto, TQueryDomain, WebApiEndpoint.ResponseRedirect, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseRedirectMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseRedirect>> ExecuteAsync(TQueryDomain request, CancellationToken cancellationToken);
            }
        }
    }
}