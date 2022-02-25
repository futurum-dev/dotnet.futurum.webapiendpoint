using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class QueryWebApiEndpoint
{
    /// <summary>
    /// Configure with request
    /// </summary>
    public abstract class WithRequest<TQueryDomain>
    {
        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class WithResponse<TResponseDto, TResponseDomain>
        {
            public abstract class WithMapper<TMapper> : IQueryWebApiEndpoint<TResponseDto, TQueryDomain, TResponseDomain, TMapper, TMapper>
                where TMapper : IWebApiEndpointRequestMapper<TQueryDomain>, IWebApiEndpointResponseMapper<TResponseDomain, TResponseDto>
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
            
            public abstract class WithMapper<TRequestMapper, TResponseMapper> : IQueryWebApiEndpoint<TResponseDto, TQueryDomain, TResponseDomain, TRequestMapper, TResponseMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
                where TResponseMapper : IWebApiEndpointResponseMapper<TResponseDomain, TResponseDto>
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
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class WithResponseAsyncEnumerable<TApiEndpoint, TDataDto, TData>
        {
            public abstract class WithMapper<TMapper> : IQueryWebApiEndpoint<ResponseAsyncEnumerableDto<TDataDto>, TQueryDomain, ResponseAsyncEnumerable<TApiEndpoint, TData>, TMapper, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TQueryDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
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
            
            public abstract class WithMapper<TRequestMapper, TResponseDataMapper> : IQueryWebApiEndpoint<ResponseAsyncEnumerableDto<TDataDto>, TQueryDomain, ResponseAsyncEnumerable<TApiEndpoint, TData>, TRequestMapper, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
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
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class WithResponseBytes<TApiEndpoint>
        {
            public abstract class WithMapper<TRequestMapper> : IQueryWebApiEndpoint<ResponseBytesDto, TQueryDomain, ResponseBytes<TApiEndpoint>, TRequestMapper, ResponseBytesMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
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
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class WithResponseDataCollection<TApiEndpoint, TDataDto, TData>
        {
            public abstract class WithMapper<TMapper> : IQueryWebApiEndpoint<ResponseDataCollectionDto<TDataDto>, TQueryDomain, ResponseDataCollection<TApiEndpoint, TData>, TMapper, ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TQueryDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
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
            
            public abstract class WithMapper<TRequestMapper, TResponseDataMapper> : IQueryWebApiEndpoint<ResponseDataCollectionDto<TDataDto>, TQueryDomain, ResponseDataCollection<TApiEndpoint, TData>, TRequestMapper, ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
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
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class WithResponseEmptyJson<TApiEndpoint>
        {
            public abstract class WithMapper<TRequestMapper> : IQueryWebApiEndpoint<ResponseEmptyJsonDto, TQueryDomain, ResponseEmptyJson<TApiEndpoint>, TRequestMapper, ResponseEmptyJsonMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
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
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class WithResponseFileStream<TApiEndpoint>
        {
            public abstract class WithMapper<TRequestMapper> : IQueryWebApiEndpoint<ResponseFileStreamDto, TQueryDomain, ResponseFileStream<TApiEndpoint>, TRequestMapper, ResponseFileStreamMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
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
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class WithResponseStream<TApiEndpoint>
        {
            public abstract class WithMapper<TRequestMapper> : IQueryWebApiEndpoint<ResponseStreamDto, TQueryDomain, ResponseStream<TApiEndpoint>, TRequestMapper, ResponseStreamMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDomain>
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
}