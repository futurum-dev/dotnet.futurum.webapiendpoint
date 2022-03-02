using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request upload file with payload
    /// </summary>
    public abstract class RequestUploadFileWithPayload<TApiEndpoint, TPayloadDto, TPayload>
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class NoResponse
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseEmptyDto, RequestUploadFileWithPayload<TApiEndpoint, TPayload>,
                ResponseEmpty, RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TRequestPayloadMapper>, ResponseEmptyMapper>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
            {
                /// <inheritdoc />
                public Task<Result<ResponseEmpty>> ExecuteCommandAsync(RequestUploadFileWithPayload<TApiEndpoint, TPayload> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(ResponseEmpty.Default);

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
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseJsonDto<TResponseDto>, RequestUploadFileWithPayload<TApiEndpoint, TPayload>,
                TResponseDomain, RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TMapper>, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>, IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
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

            public abstract class Mapper<TRequestPayloadMapper, TResponseMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseJsonDto<TResponseDto>,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>, TResponseDomain, RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TRequestPayloadMapper>, 
                ResponseJsonMapper<TResponseDomain, TResponseDto, TResponseMapper>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
                where TResponseMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
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
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TRequestPayloadMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseAsyncEnumerableDto<TDataDto>,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>, WebApiEndpoint.ResponseAsyncEnumerable<TApiEndpoint, TData>, RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TRequestPayloadMapper>,
                ResponseAsyncEnumerableMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseAsyncEnumerable<TApiEndpoint, TData>>>
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
        public abstract class ResponseBytes
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseBytesDto,
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
                protected abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseDataCollectionDto<TDataDto>,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>, WebApiEndpoint.ResponseDataCollection<TApiEndpoint, TData>, RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TMapper>,
                ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TMapper>>
                where TMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>, IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseDataCollection<TApiEndpoint, TData>>>
                    ExecuteCommandAsync(RequestUploadFileWithPayload<TApiEndpoint, TPayload> command, CancellationToken cancellationToken) =>
                    ExecuteAsync(command.ToNonApiEndpoint(), cancellationToken).MapAsync(responseDataCollection => responseDataCollection.ToApiEndpoint<TApiEndpoint>());

                /// <summary>
                /// Execute the WebApiEndpoint
                /// <para>This method is called once for each request received</para>
                /// </summary>
                protected abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }

            public abstract class Mapper<TRequestPayloadMapper, TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseDataCollectionDto<TDataDto>,
                RequestUploadFileWithPayload<TApiEndpoint, TPayload>, WebApiEndpoint.ResponseDataCollection<TApiEndpoint, TData>, RequestUploadFileWithPayloadMapper<TApiEndpoint, TPayloadDto, TPayload, TRequestPayloadMapper>,
                ResponseDataCollectionMapper<TApiEndpoint, TData, TDataDto, TResponseDataMapper>>
                where TRequestPayloadMapper : IWebApiEndpointRequestPayloadMapper<TPayloadDto, TPayload>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public Task<Result<WebApiEndpoint.ResponseDataCollection<TApiEndpoint, TData>>>
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
        public abstract class ResponseEmptyJson
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseEmptyJsonDto,
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
                protected abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseFileStreamDto,
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
                protected abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream
        {
            public abstract class Mapper<TRequestPayloadMapper> : ICommandWebApiEndpoint<RequestUploadFileWithPayloadDto<TPayloadDto>, ResponseStreamDto,
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
                protected abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(RequestUploadFileWithPayload<TPayload> command, CancellationToken cancellationToken);
            }
        }
    }
}