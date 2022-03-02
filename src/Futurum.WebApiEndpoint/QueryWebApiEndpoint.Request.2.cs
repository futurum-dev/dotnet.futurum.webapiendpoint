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
                public Task<Result<TResponseDomain>> ExecuteCommandAsync(TQueryDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<TResponseDomain>> ExecuteAsync(TQueryDomain query, CancellationToken cancellationToken);
            }
            
            public abstract class Mapper<TRequestMapper, TResponseMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseJsonDto<TResponseDto>, TQueryDomain, TResponseDomain,
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TResponseMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
                where TResponseMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto> 
            {
                /// <inheritdoc />
                public Task<Result<TResponseDomain>> ExecuteCommandAsync(TQueryDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken);

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
        public abstract class ResponseAsyncEnumerable<TApiEndpoint, TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseAsyncEnumerableDto<TDataDto>, TQueryDomain, ResponseAsyncEnumerable<TApiEndpoint, TData>, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TMapper>, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseAsyncEnumerable<TApiEndpoint, TData>>> ExecuteCommandAsync(TQueryDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseAsyncEnumerable => responseAsyncEnumerable.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(TQueryDomain query, CancellationToken cancellationToken);
            }
            
            public abstract class Mapper<TRequestMapper, TResponseDataMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseAsyncEnumerableDto<TDataDto>, TQueryDomain, ResponseAsyncEnumerable<TApiEndpoint, TData>, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseAsyncEnumerable<TApiEndpoint, TData>>> ExecuteCommandAsync(TQueryDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseAsyncEnumerable => responseAsyncEnumerable.ToApiEndpoint<TApiEndpoint>());

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
        public abstract class ResponseBytes<TApiEndpoint>
        {
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseBytesDto, TQueryDomain, WebApiEndpoint.ResponseBytes<TApiEndpoint>, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseBytesMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseBytes<TApiEndpoint>>> ExecuteCommandAsync(TQueryDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseBytes => responseBytes.ToApiEndpoint<TApiEndpoint>());

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
        public abstract class ResponseDataCollection<TApiEndpoint, TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseDataCollectionDto<TDataDto>, TQueryDomain, ResponseDataCollection<TApiEndpoint, TData>, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TMapper>, ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseDataCollection<TApiEndpoint, TData>>> ExecuteCommandAsync(TQueryDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TQueryDomain query, CancellationToken cancellationToken);
            }
            
            public abstract class Mapper<TRequestMapper, TResponseDataMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseDataCollectionDto<TDataDto>, TQueryDomain, ResponseDataCollection<TApiEndpoint, TData>, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseDataCollection<TApiEndpoint, TData>>> ExecuteCommandAsync(TQueryDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

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
        public abstract class ResponseEmptyJson<TApiEndpoint>
        {
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseEmptyJsonDto, TQueryDomain, WebApiEndpoint.ResponseEmptyJson<TApiEndpoint>, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseEmptyJsonMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseEmptyJson<TApiEndpoint>>> ExecuteCommandAsync(TQueryDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseEmptyJson => responseEmptyJson.ToApiEndpoint<TApiEndpoint>());

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
        public abstract class ResponseFileStream<TApiEndpoint>
        {
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseFileStreamDto, TQueryDomain, WebApiEndpoint.ResponseFileStream<TApiEndpoint>, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseFileStreamMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseFileStream<TApiEndpoint>>> ExecuteCommandAsync(TQueryDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseFileStream => responseFileStream.ToApiEndpoint<TApiEndpoint>());

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
        public abstract class ResponseStream<TApiEndpoint>
        {
            public abstract class Mapper<TRequestMapper> : IQueryWebApiEndpoint<RequestJsonDto<TQueryDto>, ResponseStreamDto, TQueryDomain, WebApiEndpoint.ResponseStream<TApiEndpoint>, 
                RequestJsonMapper<TQueryDto, TQueryDomain, TRequestMapper>, ResponseStreamMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TQueryDto, TQueryDomain>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseStream<TApiEndpoint>>> ExecuteCommandAsync(TQueryDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseStream => responseStream.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseStream>> ExecuteAsync(TQueryDomain query, CancellationToken cancellationToken);
            }
        }
    }
}