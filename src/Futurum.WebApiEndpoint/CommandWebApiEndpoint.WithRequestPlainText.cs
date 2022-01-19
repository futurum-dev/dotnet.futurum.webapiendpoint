using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request plain text
    /// </summary>
    public class WithRequestPlainText<TApiEndpoint>
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class WithoutResponse : ICommandWebApiEndpoint<RequestPlainTextDto, RequestPlainText<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result> ExecuteCommandAsync(RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class WithResponse<TResponseDto, TResponseDomain> : ICommandWebApiEndpoint<RequestPlainTextDto, TResponseDto, RequestPlainText<TApiEndpoint>, TResponseDomain>
        {
            /// <inheritdoc />
            public Task<Result<TResponseDomain>> ExecuteCommandAsync(RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<TResponseDomain>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class WithResponseAsyncEnumerable<TDataDto, TData> : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseAsyncEnumerableDto<TDataDto>, RequestPlainText<TApiEndpoint>,
            ResponseAsyncEnumerable<TApiEndpoint, TData>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseAsyncEnumerable<TApiEndpoint, TData>>> ExecuteCommandAsync(RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseAsyncEnumerable => responseAsyncEnumerable.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class WithResponseBytes : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseBytesDto, RequestPlainText<TApiEndpoint>, ResponseBytes<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseBytes<TApiEndpoint>>> ExecuteCommandAsync(RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseBytes => responseBytes.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseBytes>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class WithResponseDataCollection<TDataDto, TData> : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseDataCollectionDto<TDataDto>, RequestPlainText<TApiEndpoint>,
            ResponseDataCollection<TApiEndpoint, TData>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseDataCollection<TApiEndpoint, TData>>> ExecuteCommandAsync(RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class WithResponseEmptyJson : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseEmptyJsonDto, RequestPlainText<TApiEndpoint>, ResponseEmptyJson<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseEmptyJson<TApiEndpoint>>> ExecuteCommandAsync(RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseEmptyJson => responseEmptyJson.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseEmptyJson>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class WithResponseFileStream : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseFileStreamDto, RequestPlainText<TApiEndpoint>, ResponseFileStream<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseFileStream<TApiEndpoint>>> ExecuteCommandAsync(RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseFileStream => responseFileStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseFileStream>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class WithResponseStream : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseStreamDto, RequestPlainText<TApiEndpoint>, ResponseStream<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseStream<TApiEndpoint>>> ExecuteCommandAsync(RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseStream => responseStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<ResponseStream>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }
    }
}