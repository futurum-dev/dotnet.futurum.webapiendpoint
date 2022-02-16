using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request
    /// </summary>
    public class WithRequest<TCommandDto, TCommandDomain>
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class WithoutResponse
        {
            public abstract class WithMapper<TRequestMapper> : ICommandWebApiEndpoint<TCommandDto, TCommandDomain, TRequestMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
            {
                /// <inheritdoc />
                public Task<Result> ExecuteCommandAsync(TCommandDomain command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken);

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
            public abstract class WithMapper<TMapper> : ICommandWebApiEndpoint<TCommandDto, TResponseDto, TCommandDomain, TResponseDomain, TMapper, TMapper>
                where TMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>, IWebApiEndpointResponseMapper<TResponseDomain, TResponseDto>
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
            
            public abstract class WithMapper<TRequestMapper, TResponseMapper> : ICommandWebApiEndpoint<TCommandDto, TResponseDto, TCommandDomain, TResponseDomain, TRequestMapper, TResponseMapper>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
                where TResponseMapper : IWebApiEndpointResponseMapper<TResponseDomain, TResponseDto>
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
            public abstract class WithMapper<TRequestMapper> : ICommandWebApiEndpoint<TCommandDto, ResponseAsyncEnumerableDto<TDataDto>, TCommandDomain, ResponseAsyncEnumerable<TApiEndpoint, TData>, TRequestMapper, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
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
            public abstract class WithMapper<TRequestMapper> : ICommandWebApiEndpoint<TCommandDto, ResponseBytesDto, TCommandDomain, ResponseBytes<TApiEndpoint>, TRequestMapper, ResponseBytesMapper<TApiEndpoint>>
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
            public abstract class WithMapper<TRequestMapper> : ICommandWebApiEndpoint<TCommandDto, ResponseDataCollectionDto<TDataDto>, TCommandDomain, ResponseDataCollection<TApiEndpoint, TData>, TRequestMapper, ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto>>
                where TRequestMapper : IWebApiEndpointRequestMapper<TCommandDto, TCommandDomain>
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
            public abstract class WithMapper<TRequestMapper> : ICommandWebApiEndpoint<TCommandDto, ResponseEmptyJsonDto, TCommandDomain, ResponseEmptyJson<TApiEndpoint>, TRequestMapper, ResponseEmptyJsonMapper<TApiEndpoint>>
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
            public abstract class WithMapper<TRequestMapper> : ICommandWebApiEndpoint<TCommandDto, ResponseFileStreamDto, TCommandDomain, ResponseFileStream<TApiEndpoint>, TRequestMapper, ResponseFileStreamMapper<TApiEndpoint>>
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
            public abstract class WithMapper<TRequestMapper> : ICommandWebApiEndpoint<TCommandDto, ResponseStreamDto, TCommandDomain, ResponseStream<TApiEndpoint>, TRequestMapper, ResponseStreamMapper<TApiEndpoint>>
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