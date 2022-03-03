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
                public Task<Result<TResponseDomain>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                    ExecuteAsync(cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<TResponseDomain>> ExecuteAsync(CancellationToken cancellationToken);
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
                public Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                    ExecuteAsync(cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes : IQueryWebApiEndpoint<RequestEmptyDto, ResponseBytesDto, RequestEmpty, WebApiEndpoint.ResponseBytes, RequestEmptyMapper, ResponseBytesMapper>
        {
            /// <inheritdoc />
            public Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                ExecuteAsync(cancellationToken);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(CancellationToken cancellationToken);
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
                public Task<Result<ResponseDataCollection<TData>>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                    ExecuteAsync(cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson : IQueryWebApiEndpoint<RequestEmptyDto, ResponseEmptyJsonDto, RequestEmpty, WebApiEndpoint.ResponseEmptyJson, 
            RequestEmptyMapper, ResponseEmptyJsonMapper>
        {
            /// <inheritdoc />
            public Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                ExecuteAsync(cancellationToken);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream : IQueryWebApiEndpoint<RequestEmptyDto, ResponseFileStreamDto, RequestEmpty, WebApiEndpoint.ResponseFileStream, 
            RequestEmptyMapper, ResponseFileStreamMapper>
        {
            /// <inheritdoc />
            public Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                ExecuteAsync(cancellationToken);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream : IQueryWebApiEndpoint<RequestEmptyDto, ResponseStreamDto, RequestEmpty, WebApiEndpoint.ResponseStream, 
            RequestEmptyMapper, ResponseStreamMapper>
        {
            /// <inheritdoc />
            public Task<Result<WebApiEndpoint.ResponseStream>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                ExecuteAsync(cancellationToken);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(CancellationToken cancellationToken);
        }
    }
}