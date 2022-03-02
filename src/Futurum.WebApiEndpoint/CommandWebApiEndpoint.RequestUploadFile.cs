using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request upload file
    /// </summary>
    public abstract class RequestUploadFile<TApiEndpoint>
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class NoResponse : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseEmptyDto, WebApiEndpoint.RequestUploadFile<TApiEndpoint>, ResponseEmpty,
            RequestUploadFileMapper<TApiEndpoint>, ResponseEmptyMapper>
        {
            /// <inheritdoc />
            public Task<Result<ResponseEmpty>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFile<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(ResponseEmpty.Default);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseJsonDto<TResponseDto>, WebApiEndpoint.RequestUploadFile<TApiEndpoint>, TResponseDomain, 
                RequestUploadFileMapper<TApiEndpoint>, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public Task<Result<TResponseDomain>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFile<TApiEndpoint> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<TResponseDomain>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseAsyncEnumerableDto<TDataDto>, WebApiEndpoint.RequestUploadFile<TApiEndpoint>, WebApiEndpoint.ResponseAsyncEnumerable<TApiEndpoint, TData>, RequestUploadFileMapper<TApiEndpoint>, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseAsyncEnumerable<TApiEndpoint, TData>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFile<TApiEndpoint> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseAsyncEnumerable => responseAsyncEnumerable.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseBytesDto, WebApiEndpoint.RequestUploadFile<TApiEndpoint>, ResponseBytes<TApiEndpoint>,
            RequestUploadFileMapper<TApiEndpoint>, ResponseBytesMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseBytes<TApiEndpoint>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFile<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseBytes => responseBytes.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseDataCollectionDto<TDataDto>, WebApiEndpoint.RequestUploadFile<TApiEndpoint>, WebApiEndpoint.ResponseDataCollection<TApiEndpoint, TData>, RequestUploadFileMapper<TApiEndpoint>, ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseDataCollection<TApiEndpoint, TData>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFile<TApiEndpoint> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseEmptyJsonDto, WebApiEndpoint.RequestUploadFile<TApiEndpoint>, ResponseEmptyJson<TApiEndpoint>,
            RequestUploadFileMapper<TApiEndpoint>, ResponseEmptyJsonMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseEmptyJson<TApiEndpoint>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFile<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseEmptyJson => responseEmptyJson.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseFileStreamDto, WebApiEndpoint.RequestUploadFile<TApiEndpoint>, ResponseFileStream<TApiEndpoint>,
            RequestUploadFileMapper<TApiEndpoint>, ResponseFileStreamMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseFileStream<TApiEndpoint>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFile<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseFileStream => responseFileStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseStreamDto, WebApiEndpoint.RequestUploadFile<TApiEndpoint>, ResponseStream<TApiEndpoint>,
            RequestUploadFileMapper<TApiEndpoint>, ResponseStreamMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseStream<TApiEndpoint>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFile<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseStream => responseStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken);
        }
    }
}