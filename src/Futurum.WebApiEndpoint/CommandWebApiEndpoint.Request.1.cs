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
                public Task<Result<ResponseEmpty>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(ResponseEmpty.Default);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
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
                public Task<Result<TResponseDomain>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<TResponseDomain>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestMapper, TResponseMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseJsonDto<TResponseDto>, TCommandDomain, TResponseDomain, 
                RequestEmptyMapper<TCommandDomain, TRequestMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TResponseMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
                where TResponseMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public Task<Result<TResponseDomain>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<TResponseDomain>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TApiEndpoint, TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseAsyncEnumerableDto<TDataDto>, TCommandDomain, ResponseAsyncEnumerable<TApiEndpoint, TData>, 
                    RequestEmptyMapper<TCommandDomain, TMapper>, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TCommandDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseAsyncEnumerable<TApiEndpoint, TData>>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseAsyncEnumerable => responseAsyncEnumerable.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseAsyncEnumerableDto<TDataDto>, TCommandDomain,
                ResponseAsyncEnumerable<TApiEndpoint, TData>, RequestEmptyMapper<TCommandDomain, TRequestMapper>, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseAsyncEnumerable<TApiEndpoint, TData>>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseAsyncEnumerable => responseAsyncEnumerable.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes<TApiEndpoint>
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseBytesDto, TCommandDomain, WebApiEndpoint.ResponseBytes<TApiEndpoint>, 
                RequestEmptyMapper<TCommandDomain, TRequestMapper>, ResponseBytesMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseBytes<TApiEndpoint>>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseBytes => responseBytes.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseBytes>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TApiEndpoint, TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseDataCollectionDto<TDataDto>, TCommandDomain, ResponseDataCollection<TApiEndpoint, TData>, 
                RequestEmptyMapper<TCommandDomain, TMapper>, ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TCommandDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseDataCollection<TApiEndpoint, TData>>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseDataCollectionDto<TDataDto>, TCommandDomain,
                ResponseDataCollection<TApiEndpoint, TData>, RequestEmptyMapper<TCommandDomain,TRequestMapper>,
                ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseDataCollection<TApiEndpoint, TData>>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson<TApiEndpoint>
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseEmptyJsonDto, TCommandDomain, WebApiEndpoint.ResponseEmptyJson<TApiEndpoint>, 
                RequestEmptyMapper<TCommandDomain,TRequestMapper>, ResponseEmptyJsonMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseEmptyJson<TApiEndpoint>>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseEmptyJson => responseEmptyJson.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseEmptyJson>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream<TApiEndpoint>
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseFileStreamDto, TCommandDomain, WebApiEndpoint.ResponseFileStream<TApiEndpoint>, 
                RequestEmptyMapper<TCommandDomain,TRequestMapper>, ResponseFileStreamMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseFileStream<TApiEndpoint>>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseFileStream => responseFileStream.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseFileStream>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream<TApiEndpoint>
        {
            public abstract class Mapper<TRequestMapper> : ICommandWebApiEndpoint<RequestEmptyDto, ResponseStreamDto, TCommandDomain, WebApiEndpoint.ResponseStream<TApiEndpoint>, 
                    RequestEmptyMapper<TCommandDomain,TRequestMapper>, ResponseStreamMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDomain>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseStream<TApiEndpoint>>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken).MapAsync(responseStream => responseStream.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseStream>> ExecuteAsync(TCommandDomain command, CancellationToken cancellationToken);
            }
        }
    }
}