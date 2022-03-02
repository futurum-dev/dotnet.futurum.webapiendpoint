using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request plain text
    /// </summary>
    public abstract class RequestPlainText<TApiEndpoint>
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class NoResponse : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseEmptyDto, WebApiEndpoint.RequestPlainText<TApiEndpoint>, ResponseEmpty, RequestPlainTextMapper<TApiEndpoint>, ResponseEmptyMapper>
        {
            /// <inheritdoc />
            public Task<Result<ResponseEmpty>> ExecuteCommandAsync(WebApiEndpoint.RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(ResponseEmpty.Default);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseJsonDto<TResponseDto>, WebApiEndpoint.RequestPlainText<TApiEndpoint>, TResponseDomain, 
                RequestPlainTextMapper<TApiEndpoint>, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public Task<Result<TResponseDomain>> ExecuteCommandAsync(WebApiEndpoint.RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<TResponseDomain>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseAsyncEnumerableDto<TDataDto>, WebApiEndpoint.RequestPlainText<TApiEndpoint>, WebApiEndpoint.ResponseAsyncEnumerable<TApiEndpoint, TData>, RequestPlainTextMapper<TApiEndpoint>, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseAsyncEnumerable<TApiEndpoint, TData>>> ExecuteCommandAsync(WebApiEndpoint.RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseAsyncEnumerable => responseAsyncEnumerable.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseBytesDto, WebApiEndpoint.RequestPlainText<TApiEndpoint>, ResponseBytes<TApiEndpoint>,
            RequestPlainTextMapper<TApiEndpoint>, ResponseBytesMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseBytes<TApiEndpoint>>> ExecuteCommandAsync(WebApiEndpoint.RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseBytes => responseBytes.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseDataCollectionDto<TDataDto>, WebApiEndpoint.RequestPlainText<TApiEndpoint>, WebApiEndpoint.ResponseDataCollection<TApiEndpoint, TData>, RequestPlainTextMapper<TApiEndpoint>, ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseDataCollection<TApiEndpoint, TData>>> ExecuteCommandAsync(WebApiEndpoint.RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseEmptyJsonDto, WebApiEndpoint.RequestPlainText<TApiEndpoint>, ResponseEmptyJson<TApiEndpoint>,
            RequestPlainTextMapper<TApiEndpoint>, ResponseEmptyJsonMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseEmptyJson<TApiEndpoint>>> ExecuteCommandAsync(WebApiEndpoint.RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseEmptyJson => responseEmptyJson.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseFileStreamDto, WebApiEndpoint.RequestPlainText<TApiEndpoint>, ResponseFileStream<TApiEndpoint>,
            RequestPlainTextMapper<TApiEndpoint>, ResponseFileStreamMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseFileStream<TApiEndpoint>>> ExecuteCommandAsync(WebApiEndpoint.RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseFileStream => responseFileStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream : ICommandWebApiEndpoint<RequestPlainTextDto, ResponseStreamDto, WebApiEndpoint.RequestPlainText<TApiEndpoint>, ResponseStream<TApiEndpoint>,
            RequestPlainTextMapper<TApiEndpoint>, ResponseStreamMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseStream<TApiEndpoint>>> ExecuteCommandAsync(WebApiEndpoint.RequestPlainText<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseStream => responseStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken);
        }
    }
}