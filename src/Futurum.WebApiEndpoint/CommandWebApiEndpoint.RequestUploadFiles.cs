using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request upload files
    /// </summary>
    public abstract class RequestUploadFiles
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class NoResponse : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseEmptyDto, WebApiEndpoint.RequestUploadFiles, ResponseEmpty,
            RequestUploadFilesMapper, ResponseEmptyMapper>
        {
            /// <inheritdoc />
            public Task<Result<ResponseEmpty>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken) =>
                ExecuteAsync(command, cancellationToken).MapAsync(ResponseEmpty.Default);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result> ExecuteAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseJsonDto<TResponseDto>, WebApiEndpoint.RequestUploadFiles, TResponseDomain, 
                RequestUploadFilesMapper, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public Task<Result<TResponseDomain>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<TResponseDomain>> ExecuteAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseAsyncEnumerableDto<TDataDto>, WebApiEndpoint.RequestUploadFiles, ResponseAsyncEnumerable<TData>, RequestUploadFilesMapper, ResponseAsyncEnumerableMapper<TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseBytesDto, WebApiEndpoint.RequestUploadFiles, WebApiEndpoint.ResponseBytes,
            RequestUploadFilesMapper, ResponseBytesMapper>
        {
            /// <inheritdoc />
            public Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken) =>
                ExecuteAsync(command, cancellationToken);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseDataCollectionDto<TDataDto>, WebApiEndpoint.RequestUploadFiles, ResponseDataCollection<TData>, RequestUploadFilesMapper, ResponseDataCollectionMapper<TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseDataCollection<TData>>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command, cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseEmptyJsonDto, WebApiEndpoint.RequestUploadFiles, WebApiEndpoint.ResponseEmptyJson,
            RequestUploadFilesMapper, ResponseEmptyJsonMapper>
        {
            /// <inheritdoc />
            public Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken) =>
                ExecuteAsync(command, cancellationToken);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseFileStreamDto, WebApiEndpoint.RequestUploadFiles, WebApiEndpoint.ResponseFileStream,
            RequestUploadFilesMapper, ResponseFileStreamMapper>
        {
            /// <inheritdoc />
            public Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken) =>
                ExecuteAsync(command, cancellationToken);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream : ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseStreamDto, WebApiEndpoint.RequestUploadFiles, WebApiEndpoint.ResponseStream,
            RequestUploadFilesMapper, ResponseStreamMapper>
        {
            /// <inheritdoc />
            public Task<Result<WebApiEndpoint.ResponseStream>> ExecuteCommandAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken) =>
                ExecuteAsync(command, cancellationToken);

            /// <summary>
            /// Execute the WebApiEndpoint
            /// <para>This method is called once for each request received</para>
            /// </summary>
            protected abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(WebApiEndpoint.RequestUploadFiles command, CancellationToken cancellationToken);
        }
    }
}