using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request
    /// </summary>
    public abstract class Request<TCommandDto, TCommandDomain>
        where TCommandDto : class
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class NoResponse
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseEmptyDto, TCommandDomain, ResponseEmpty,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseEmptyMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseEmpty>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseJsonDto<TResponseDto>, TCommandDomain, TResponseDomain,
                RequestJsonMapper<TCommandDto, TCommandDomain, TMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>, IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestMapper, TResponseMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseJsonDto<TResponseDto>, TCommandDomain, TResponseDomain,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TResponseMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
                where TResponseMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseAsyncEnumerableDto<TDataDto>, TCommandDomain, ResponseAsyncEnumerable<TData>,
                RequestJsonMapper<TCommandDto, TCommandDomain, TMapper>, ResponseAsyncEnumerableMapper<TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseAsyncEnumerableDto<TDataDto>, TCommandDomain,
                ResponseAsyncEnumerable<TData>,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseAsyncEnumerableMapper<TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseBytesDto, TCommandDomain, WebApiEndpoint.ResponseBytes,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseBytesMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseDataCollectionDto<TDataDto>, TCommandDomain, ResponseDataCollection<TData>,
                RequestJsonMapper<TCommandDto, TCommandDomain, TMapper>, ResponseDataCollectionMapper<TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseDataCollectionDto<TDataDto>, TCommandDomain,
                ResponseDataCollection<TData>,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseDataCollectionMapper<TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseEmptyJsonDto, TCommandDomain, WebApiEndpoint.ResponseEmptyJson,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseEmptyJsonMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseFileStreamDto, TCommandDomain, WebApiEndpoint.ResponseFileStream,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseFileStreamMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseStreamDto, TCommandDomain, WebApiEndpoint.ResponseStream,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseStreamMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
            {
                /// <inheritdoc />
                public abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }
    }
}