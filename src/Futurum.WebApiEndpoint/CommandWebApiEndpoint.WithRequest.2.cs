using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request
    /// </summary>
    public abstract class WithRequest<TCommandDto, TCommandDomain>
        where TCommandDto : class
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class WithoutResponse
        {
            public abstract class WithMapper<TRequestMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseEmptyDto, TCommandDomain, ResponseEmpty,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseEmptyMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
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
        public abstract class WithResponse<TResponseDto, TResponseDomain>
        {
            public abstract class WithMapper<TMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseJsonDto<TResponseDto>, TCommandDomain, TResponseDomain,
                RequestJsonMapper<TCommandDto, TCommandDomain, TMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>, IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
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

            public abstract class WithMapper<TRequestMapper, TResponseMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseJsonDto<TResponseDto>, TCommandDomain, TResponseDomain,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TResponseMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
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
        public abstract class WithResponseAsyncEnumerable<TApiEndpoint, TDataDto, TData>
        {
            public abstract class WithMapper<TMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseAsyncEnumerableDto<TDataDto>, TCommandDomain, ResponseAsyncEnumerable<TApiEndpoint, TData>,
                RequestJsonMapper<TCommandDto, TCommandDomain, TMapper>, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
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

            public abstract class WithMapper<TRequestMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseAsyncEnumerableDto<TDataDto>, TCommandDomain,
                ResponseAsyncEnumerable<TApiEndpoint, TData>,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
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
        public abstract class WithResponseBytes<TApiEndpoint>
        {
            public abstract class WithMapper<TRequestMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseBytesDto, TCommandDomain, ResponseBytes<TApiEndpoint>,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseBytesMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
            {
                /// <inheritdoc />
                public Task<Result<ResponseBytes<TApiEndpoint>>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
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
        public abstract class WithResponseDataCollection<TApiEndpoint, TDataDto, TData>
        {
            public abstract class WithMapper<TMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseDataCollectionDto<TDataDto>, TCommandDomain, ResponseDataCollection<TApiEndpoint, TData>,
                RequestJsonMapper<TCommandDto, TCommandDomain, TMapper>, ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
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

            public abstract class WithMapper<TRequestMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseDataCollectionDto<TDataDto>, TCommandDomain,
                ResponseDataCollection<TApiEndpoint, TData>,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
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
        public abstract class WithResponseEmptyJson<TApiEndpoint>
        {
            public abstract class WithMapper<TRequestMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseEmptyJsonDto, TCommandDomain, ResponseEmptyJson<TApiEndpoint>,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseEmptyJsonMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
            {
                /// <inheritdoc />
                public Task<Result<ResponseEmptyJson<TApiEndpoint>>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
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
        public abstract class WithResponseFileStream<TApiEndpoint>
        {
            public abstract class WithMapper<TRequestMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseFileStreamDto, TCommandDomain, ResponseFileStream<TApiEndpoint>,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseFileStreamMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
            {
                /// <inheritdoc />
                public Task<Result<ResponseFileStream<TApiEndpoint>>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
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
        public abstract class WithResponseStream<TApiEndpoint>
        {
            public abstract class WithMapper<TRequestMapper> : ICommandWebApiEndpoint<RequestJsonDto<TCommandDto>, ResponseStreamDto, TCommandDomain, ResponseStream<TApiEndpoint>,
                RequestJsonMapper<TCommandDto, TCommandDomain, TRequestMapper>, ResponseStreamMapper<TApiEndpoint>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
            {
                /// <inheritdoc />
                public Task<Result<ResponseStream<TApiEndpoint>>> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
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