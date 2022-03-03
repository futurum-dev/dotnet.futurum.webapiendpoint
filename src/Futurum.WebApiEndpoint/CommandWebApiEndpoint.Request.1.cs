using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Builder for <see cref="ICommandWebApiEndpoint"/>
/// </summary>
public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request
    /// </summary>
    public abstract class Request<TCommandDomain>
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class NoResponse
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseEmptyDto, TCommandDomain, ResponseEmpty, 
                RequestEmptyMapper<TCommandDomain, TRequestMapper>, ResponseEmptyMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseEmpty>> ExecuteAsync(TCommandDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseJsonDto<TResponseDto>, TCommandDomain, TResponseDomain, 
                RequestEmptyMapper<TCommandDomain, TMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TCommandDomain>, IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(TCommandDomain request, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestMapper, TResponseMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseJsonDto<TResponseDto>, TCommandDomain, TResponseDomain, 
                RequestEmptyMapper<TCommandDomain, TRequestMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TResponseMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
                where TResponseMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(TCommandDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseAsyncEnumerableDto<TDataDto>, TCommandDomain, ResponseAsyncEnumerable<TData>, 
                    RequestEmptyMapper<TCommandDomain, TMapper>, ResponseAsyncEnumerableMapper<TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TCommandDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(TCommandDomain request, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseAsyncEnumerableDto<TDataDto>, TCommandDomain,
                ResponseAsyncEnumerable<TData>, RequestEmptyMapper<TCommandDomain, TRequestMapper>, ResponseAsyncEnumerableMapper<TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(TCommandDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseBytesDto, TCommandDomain, WebApiEndpoint.ResponseBytes, 
                RequestEmptyMapper<TCommandDomain, TRequestMapper>, ResponseBytesMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(TCommandDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseDataCollectionDto<TDataDto>, TCommandDomain, ResponseDataCollection<TData>, 
                RequestEmptyMapper<TCommandDomain, TMapper>, ResponseDataCollectionMapper<TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TCommandDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TCommandDomain request, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseDataCollectionDto<TDataDto>, TCommandDomain,
                ResponseDataCollection<TData>, RequestEmptyMapper<TCommandDomain,TRequestMapper>,
                ResponseDataCollectionMapper<TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TCommandDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseEmptyJsonDto, TCommandDomain, WebApiEndpoint.ResponseEmptyJson, 
                RequestEmptyMapper<TCommandDomain,TRequestMapper>, ResponseEmptyJsonMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(TCommandDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseFileStreamDto, TCommandDomain, WebApiEndpoint.ResponseFileStream, 
                RequestEmptyMapper<TCommandDomain,TRequestMapper>, ResponseFileStreamMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(TCommandDomain request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseStreamDto, TCommandDomain, WebApiEndpoint.ResponseStream, 
                    RequestEmptyMapper<TCommandDomain,TRequestMapper>, ResponseStreamMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(TCommandDomain request, CancellationToken cancellationToken);
            }
        }
    }
}