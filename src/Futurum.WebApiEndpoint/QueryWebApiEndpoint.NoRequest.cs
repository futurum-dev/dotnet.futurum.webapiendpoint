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
        public abstract class ResponseAsyncEnumerable<TApiEndpoint, TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseAsyncEnumerableDto<TDataDto>, RequestEmpty, ResponseAsyncEnumerable<TApiEndpoint, TData>, 
                RequestEmptyMapper, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseAsyncEnumerable<TApiEndpoint, TData>>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                    ExecuteAsync(cancellationToken).MapAsync(responseAsyncEnumerable => responseAsyncEnumerable.ToApiEndpoint<TApiEndpoint>());

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
        public abstract class ResponseBytes<TApiEndpoint> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseBytesDto, RequestEmpty, WebApiEndpoint.ResponseBytes<TApiEndpoint>, 
            RequestEmptyMapper, ResponseBytesMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<WebApiEndpoint.ResponseBytes<TApiEndpoint>>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                ExecuteAsync(cancellationToken).MapAsync(responseBytes => responseBytes.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseBytes>> ExecuteAsync(CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TApiEndpoint, TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseDataCollectionDto<TDataDto>, RequestEmpty, ResponseDataCollection<TApiEndpoint, TData>, 
                RequestEmptyMapper, ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseDataCollection<TApiEndpoint, TData>>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                    ExecuteAsync(cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

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
        public abstract class ResponseEmptyJson<TApiEndpoint> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseEmptyJsonDto, RequestEmpty, WebApiEndpoint.ResponseEmptyJson<TApiEndpoint>, 
            RequestEmptyMapper, ResponseEmptyJsonMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<WebApiEndpoint.ResponseEmptyJson<TApiEndpoint>>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                ExecuteAsync(cancellationToken).MapAsync(responseEmptyJson => responseEmptyJson.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseEmptyJson>> ExecuteAsync(CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream<TApiEndpoint> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseFileStreamDto, RequestEmpty, WebApiEndpoint.ResponseFileStream<TApiEndpoint>, 
            RequestEmptyMapper, ResponseFileStreamMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<WebApiEndpoint.ResponseFileStream<TApiEndpoint>>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                ExecuteAsync(cancellationToken).MapAsync(responseFileStream => responseFileStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseFileStream>> ExecuteAsync(CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream<TApiEndpoint> : IQueryWebApiEndpoint<RequestEmptyDto, ResponseStreamDto, RequestEmpty, WebApiEndpoint.ResponseStream<TApiEndpoint>, 
            RequestEmptyMapper, ResponseStreamMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<WebApiEndpoint.ResponseStream<TApiEndpoint>>> ExecuteCommandAsync(RequestEmpty command, CancellationToken cancellationToken) =>
                ExecuteAsync(cancellationToken).MapAsync(responseStream => responseStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseStream>> ExecuteAsync(CancellationToken cancellationToken);
        }
    }
}