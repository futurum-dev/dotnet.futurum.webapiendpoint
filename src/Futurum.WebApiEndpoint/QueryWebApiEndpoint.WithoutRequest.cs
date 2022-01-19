using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Builder for <see cref="IQueryWebApiEndpoint"/>
/// </summary>
public partial class QueryWebApiEndpoint
{
    /// <summary>
    /// Configure without request
    /// </summary>
    public class WithoutRequest
    {
        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class WithResponse<TResponseDto, TResponseDomain> : IQueryWebApiEndpoint<TResponseDto, TResponseDomain>
        {
            /// <inheritdoc />
            public Task<Result<TResponseDomain>> ExecuteQueryAsync(CancellationToken cancellationToken) =>
                ExecuteAsync(cancellationToken);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<TResponseDomain>> ExecuteAsync(CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class WithResponseAsyncEnumerable<TApiEndpoint, TDataDto, TData> : IQueryWebApiEndpoint<ResponseAsyncEnumerableDto<TDataDto>, ResponseAsyncEnumerable<TApiEndpoint, TData>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseAsyncEnumerable<TApiEndpoint, TData>>> ExecuteQueryAsync(CancellationToken cancellationToken) =>
                ExecuteAsync(cancellationToken).MapAsync(responseAsyncEnumerable => responseAsyncEnumerable.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class WithResponseBytes<TApiEndpoint> : IQueryWebApiEndpoint<ResponseBytesDto, ResponseBytes<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseBytes<TApiEndpoint>>> ExecuteQueryAsync(CancellationToken cancellationToken) =>
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
        public abstract class WithResponseDataCollection<TApiEndpoint, TDataDto, TData> : IQueryWebApiEndpoint<ResponseDataCollectionDto<TDataDto>, ResponseDataCollection<TApiEndpoint, TData>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseDataCollection<TApiEndpoint, TData>>> ExecuteQueryAsync(CancellationToken cancellationToken) =>
                ExecuteAsync(cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class WithResponseEmptyJson<TApiEndpoint> : IQueryWebApiEndpoint<ResponseEmptyJsonDto, ResponseEmptyJson<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseEmptyJson<TApiEndpoint>>> ExecuteQueryAsync(CancellationToken cancellationToken) =>
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
        public abstract class WithResponseFileStream<TApiEndpoint> : IQueryWebApiEndpoint<ResponseFileStreamDto, ResponseFileStream<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseFileStream<TApiEndpoint>>> ExecuteQueryAsync(CancellationToken cancellationToken) =>
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
        public abstract class WithResponseStream<TApiEndpoint> : IQueryWebApiEndpoint<ResponseStreamDto, ResponseStream<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseStream<TApiEndpoint>>> ExecuteQueryAsync(CancellationToken cancellationToken) =>
                ExecuteAsync(cancellationToken).MapAsync(responseStream => responseStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseStream>> ExecuteAsync(CancellationToken cancellationToken);
        }
    }
}