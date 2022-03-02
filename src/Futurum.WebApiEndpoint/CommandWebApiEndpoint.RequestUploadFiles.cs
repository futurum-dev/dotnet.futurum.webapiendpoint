using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request upload files
    /// </summary>
    public abstract class RequestUploadFiles<TApiEndpoint>
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class NoResponse : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseEmptyDto, WebApiEndpoint.RequestUploadFiles<TApiEndpoint>, ResponseEmpty,
            RequestUploadFilesMapper<TApiEndpoint>, ResponseEmptyMapper>
        {
            /// <inheritdoc />
            public Task<Result<ResponseEmpty>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(ResponseEmpty.Default);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseJsonDto<TResponseDto>, WebApiEndpoint.RequestUploadFiles<TApiEndpoint>, TResponseDomain, 
                RequestUploadFilesMapper<TApiEndpoint>, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public Task<Result<TResponseDomain>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles<TApiEndpoint> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<TResponseDomain>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseAsyncEnumerableDto<TDataDto>, WebApiEndpoint.RequestUploadFiles<TApiEndpoint>, WebApiEndpoint.ResponseAsyncEnumerable<TApiEndpoint, TData>, RequestUploadFilesMapper<TApiEndpoint>, ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseAsyncEnumerable<TApiEndpoint, TData>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles<TApiEndpoint> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseAsyncEnumerable => responseAsyncEnumerable.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseBytesDto, WebApiEndpoint.RequestUploadFiles<TApiEndpoint>, ResponseBytes<TApiEndpoint>,
            RequestUploadFilesMapper<TApiEndpoint>, ResponseBytesMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseBytes<TApiEndpoint>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseBytes => responseBytes.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseDataCollectionDto<TDataDto>, WebApiEndpoint.RequestUploadFiles<TApiEndpoint>, WebApiEndpoint.ResponseDataCollection<TApiEndpoint, TData>, RequestUploadFilesMapper<TApiEndpoint>, ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseDataCollection<TApiEndpoint, TData>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles<TApiEndpoint> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseEmptyJsonDto, WebApiEndpoint.RequestUploadFiles<TApiEndpoint>, ResponseEmptyJson<TApiEndpoint>,
            RequestUploadFilesMapper<TApiEndpoint>, ResponseEmptyJsonMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseEmptyJson<TApiEndpoint>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseEmptyJson => responseEmptyJson.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseFileStreamDto, WebApiEndpoint.RequestUploadFiles<TApiEndpoint>, ResponseFileStream<TApiEndpoint>,
            RequestUploadFilesMapper<TApiEndpoint>, ResponseFileStreamMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseFileStream<TApiEndpoint>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseFileStream => responseFileStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseStreamDto, WebApiEndpoint.RequestUploadFiles<TApiEndpoint>, ResponseStream<TApiEndpoint>,
            RequestUploadFilesMapper<TApiEndpoint>, ResponseStreamMapper<TApiEndpoint>>
        {
            /// <inheritdoc />
            public Task<Result<ResponseStream<TApiEndpoint>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles<TApiEndpoint> command, CancellationToken cancellationToken) =>
                ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseStream => responseStream.ToApiEndpoint<TApiEndpoint>());

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken);
        }
    }
}