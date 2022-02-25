using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request upload file with payload
    /// </summary>
    public abstract class WithRequestUploadFileWithPayload<TApiEndpoint, TPayloadDto, TPayload>
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class WithoutResponse
        {
            public abstract class WithMapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, RequestUploadFileWithPayload<TApiEndpoint, TPayload>,
                RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TRequestPayloadMapper>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
            {
                /// <inheritdoc />
                public Task<Result> ExecuteCommandAsync(RequestUploadFileWithPayload<TApiEndpoint, TPayload> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class WithResponse<TResponseDto, TResponseDomain>
        {
            public abstract class WithMapper<TMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, TResponseDto,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>,
                TResponseDomain, RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TMapper>, TMapper>
                where TMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>, IWebApiEndpointResponseMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public Task<Result<TResponseDomain>> ExecuteCommandAsync(RequestUploadFileWithPayload<TApiEndpoint, TPayload> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<TResponseDomain>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }

            public abstract class WithMapper<TRequestPayloadMapper, TResponseMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, TResponseDto,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>,
                TResponseDomain, RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TRequestPayloadMapper>, TResponseMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
                where TResponseMapper : IWebApiEndpointResponseMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public Task<Result<TResponseDomain>> ExecuteCommandAsync(RequestUploadFileWithPayload<TApiEndpoint, TPayload> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken);

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<TResponseDomain>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class WithResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class WithMapper<TRequestPayloadMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseAsyncEnumerableDto<TDataDto>,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>,
                ResponseAsyncEnumerable<TApiEndpoint, TData>, RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TRequestPayloadMapper>,
                ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseAsyncEnumerable<TApiEndpoint, TData>>>
                    ExecuteCommandAsync(RequestUploadFileWithPayload<TApiEndpoint, TPayload> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseAsyncEnumerable => responseAsyncEnumerable.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class WithResponseBytes
        {
            public abstract class WithMapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseBytesDto,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>,
                ResponseBytes<TApiEndpoint>,
                RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TRequestPayloadMapper>, ResponseBytesMapper<TApiEndpoint>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
            {
                /// <inheritdoc />
                public Task<Result<ResponseBytes<TApiEndpoint>>> ExecuteCommandAsync(RequestUploadFileWithPayload<TApiEndpoint, TPayload> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseBytes => responseBytes.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseBytes>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class WithResponseDataCollection<TDataDto, TData>
        {
            public abstract class WithMapper<TMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseDataCollectionDto<TDataDto>,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>,
                ResponseDataCollection<TApiEndpoint, TData>, RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TMapper>,
                ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseDataCollection<TApiEndpoint, TData>>>
                    ExecuteCommandAsync(RequestUploadFileWithPayload<TApiEndpoint, TPayload> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }

            public abstract class WithMapper<TRequestPayloadMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseDataCollectionDto<TDataDto>,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>,
                ResponseDataCollection<TApiEndpoint, TData>, RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TRequestPayloadMapper>,
                ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<ResponseDataCollection<TApiEndpoint, TData>>>
                    ExecuteCommandAsync(RequestUploadFileWithPayload<TApiEndpoint, TPayload> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class WithResponseEmptyJson
        {
            public abstract class WithMapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseEmptyJsonDto,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>,
                ResponseEmptyJson<TApiEndpoint>,
                RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TRequestPayloadMapper>, ResponseEmptyJsonMapper<TApiEndpoint>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
            {
                /// <inheritdoc />
                public Task<Result<ResponseEmptyJson<TApiEndpoint>>> ExecuteCommandAsync(RequestUploadFileWithPayload<TApiEndpoint, TPayload> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseEmptyJson => responseEmptyJson.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseEmptyJson>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class WithResponseFileStream
        {
            public abstract class WithMapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseFileStreamDto,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>,
                ResponseFileStream<TApiEndpoint>,
                RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TRequestPayloadMapper>, ResponseFileStreamMapper<TApiEndpoint>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
            {
                /// <inheritdoc />
                public Task<Result<ResponseFileStream<TApiEndpoint>>> ExecuteCommandAsync(RequestUploadFileWithPayload<TApiEndpoint, TPayload> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseFileStream => responseFileStream.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseFileStream>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class WithResponseStream
        {
            public abstract class WithMapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseStreamDto,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>,
                ResponseStream<TApiEndpoint>,
                RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TRequestPayloadMapper>, ResponseStreamMapper<TApiEndpoint>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
            {
                /// <inheritdoc />
                public Task<Result<ResponseStream<TApiEndpoint>>> ExecuteCommandAsync(RequestUploadFileWithPayload<TApiEndpoint, TPayload> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseStream => responseStream.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseStream>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }
        }
    }
}