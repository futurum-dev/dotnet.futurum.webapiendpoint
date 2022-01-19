using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public partial class QueryWebApiEndpoint
{
    /// <summary>
    /// Configure with request
    /// </summary>
    public class WithRequest<TQueryDomain>
    {
        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class WithResponse<TResponseDto, TResponseDomain> : IQueryWebApiEndpoint<TResponseDto, TQueryDomain, TResponseDomain>
        {
            /// <inheritdoc />
            public Task<Result<TResponseDomain>> ExecuteQueryAsync(TQueryDomain query, CancellationToken cancellationToken) =>
                ExecuteAsync(query, cancellationToken);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<TResponseDomain>> ExecuteAsync(TQueryDomain query, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class
            WithResponseAsyncEnumerable<TApiEndpoint, TDataDto, TData> : IQueryWebApiEndpoint<ResponseAsyncEnumerableDto<TDataDto>, TQueryDomain, ResponseAsyncEnumerable<TApiEndpoint, TData>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseAsyncEnumerable<TApiEndpoint, TData>>> ExecuteQueryAsync(TQueryDomain query, CancellationToken cancellationToken) =>
                ExecuteAsync(query, cancellationToken).MapAsync(responseAsyncEnumerable => responseAsyncEnumerable.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(TQueryDomain query, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class WithResponseBytes<TApiEndpoint> : IQueryWebApiEndpoint<ResponseBytesDto, TQueryDomain, ResponseBytes<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseBytes<TApiEndpoint>>> ExecuteQueryAsync(TQueryDomain query, CancellationToken cancellationToken) =>
                ExecuteAsync(query, cancellationToken).MapAsync(responseBytes => responseBytes.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseBytes>> ExecuteAsync(TQueryDomain query, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class
            WithResponseDataCollection<TApiEndpoint, TDataDto, TData> : IQueryWebApiEndpoint<ResponseDataCollectionDto<TDataDto>, TQueryDomain, ResponseDataCollection<TApiEndpoint, TData>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseDataCollection<TApiEndpoint, TData>>> ExecuteQueryAsync(TQueryDomain query, CancellationToken cancellationToken) =>
                ExecuteAsync(query, cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TQueryDomain query, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class WithResponseEmptyJson<TApiEndpoint> : IQueryWebApiEndpoint<ResponseEmptyJsonDto, TQueryDomain, ResponseEmptyJson<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseEmptyJson<TApiEndpoint>>> ExecuteQueryAsync(TQueryDomain query, CancellationToken cancellationToken) =>
                ExecuteAsync(query, cancellationToken).MapAsync(responseEmptyJson => responseEmptyJson.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseEmptyJson>> ExecuteAsync(TQueryDomain query, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class WithResponseFileStream<TApiEndpoint> : IQueryWebApiEndpoint<ResponseFileStreamDto, TQueryDomain, ResponseFileStream<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseFileStream<TApiEndpoint>>> ExecuteQueryAsync(TQueryDomain query, CancellationToken cancellationToken) =>
                ExecuteAsync(query, cancellationToken).MapAsync(responseFileStream => responseFileStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseFileStream>> ExecuteAsync(TQueryDomain query, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class WithResponseStream<TApiEndpoint> : IQueryWebApiEndpoint<ResponseStreamDto, TQueryDomain, ResponseStream<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseStream<TApiEndpoint>>> ExecuteQueryAsync(TQueryDomain query, CancellationToken cancellationToken) =>
                ExecuteAsync(query, cancellationToken).MapAsync(responseStream => responseStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseStream>> ExecuteAsync(TQueryDomain query, CancellationToken cancellationToken);
        }
    }
}