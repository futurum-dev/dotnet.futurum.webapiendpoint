using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// Builder for <see cref="ICommandWebApiEndpoint"/>
/// </summary>
public partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request
    /// </summary>
    public class WithRequest<TCommandDomain>
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class WithoutResponse : ICommandWebApiEndpoint<TCommandDomain>
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

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class WithResponse<TResponseDto, TResponseDomain> : ICommandWebApiEndpoint<TResponseDto, TCommandDomain, TResponseDomain>
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

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class
            WithResponseAsyncEnumerable<TApiEndpoint, TDataDto, TData> : ICommandWebApiEndpoint<ResponseAsyncEnumerableDto<TDataDto>, TCommandDomain, ResponseAsyncEnumerable<TApiEndpoint, TData>>
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

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class WithResponseBytes<TApiEndpoint> : ICommandWebApiEndpoint<ResponseBytesDto, TCommandDomain, ResponseBytes<TApiEndpoint>>
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

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class
            WithResponseDataCollection<TApiEndpoint, TDataDto, TData> : ICommandWebApiEndpoint<ResponseDataCollectionDto<TDataDto>, TCommandDomain, ResponseDataCollection<TApiEndpoint, TData>>
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

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class WithResponseEmptyJson<TApiEndpoint> : ICommandWebApiEndpoint<ResponseEmptyJsonDto, TCommandDomain, ResponseEmptyJson<TApiEndpoint>>
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

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class WithResponseFileStream<TApiEndpoint> : ICommandWebApiEndpoint<ResponseFileStreamDto, TCommandDomain, ResponseFileStream<TApiEndpoint>>
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

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class WithResponseStream<TApiEndpoint> : ICommandWebApiEndpoint<ResponseStreamDto, TCommandDomain, ResponseStream<TApiEndpoint>>
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