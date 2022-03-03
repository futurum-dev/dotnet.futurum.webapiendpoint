using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request plain text
    /// </summary>
    public abstract class RequestPlainText
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class NoResponse : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseEmptyDto, WebApiEndpoint.RequestPlainText, ResponseEmpty, RequestPlainTextMapper, ResponseEmptyMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<ResponseEmpty>> ExecuteAsync(WebApiEndpoint.RequestPlainText request, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseJsonDto<TResponseDto>, WebApiEndpoint.RequestPlainText, TResponseDomain, 
                RequestPlainTextMapper, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(WebApiEndpoint.RequestPlainText request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseAsyncEnumerableDto<TDataDto>, WebApiEndpoint.RequestPlainText, ResponseAsyncEnumerable<TData>, RequestPlainTextMapper, ResponseAsyncEnumerableMapper<TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(WebApiEndpoint.RequestPlainText request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseBytesDto, WebApiEndpoint.RequestPlainText, WebApiEndpoint.ResponseBytes,
            RequestPlainTextMapper, ResponseBytesMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(WebApiEndpoint.RequestPlainText request, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseDataCollectionDto<TDataDto>, WebApiEndpoint.RequestPlainText, ResponseDataCollection<TData>, RequestPlainTextMapper, ResponseDataCollectionMapper<TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(WebApiEndpoint.RequestPlainText request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseEmptyJsonDto, WebApiEndpoint.RequestPlainText, WebApiEndpoint.ResponseEmptyJson,
            RequestPlainTextMapper, ResponseEmptyJsonMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(WebApiEndpoint.RequestPlainText request, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseFileStreamDto, WebApiEndpoint.RequestPlainText, WebApiEndpoint.ResponseFileStream,
            RequestPlainTextMapper, ResponseFileStreamMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(WebApiEndpoint.RequestPlainText request, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseStreamDto, WebApiEndpoint.RequestPlainText, WebApiEndpoint.ResponseStream,
            RequestPlainTextMapper, ResponseStreamMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(WebApiEndpoint.RequestPlainText request, CancellationToken cancellationToken);
        }
    }
}