using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint;

public abstract partial class CommandWebApiEndpoint
{
    /// <summary>
    /// Configure with request upload file
    /// </summary>
    public abstract class RequestUploadFile
    {
        /// <summary>
        /// Configure without response
        /// </summary>
        public abstract class NoResponse : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseEmptyDto, WebApiEndpoint.RequestUploadFile, ResponseEmpty, RequestUploadFileMapper, ResponseEmptyMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<ResponseEmpty>> ExecuteAsync(WebApiEndpoint.RequestUploadFile request, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response
        /// </summary>
        public abstract class Response<TResponseDto, TResponseDomain>
        {
            public abstract class Mapper<TMapper> : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseJsonDto<TResponseDto>, WebApiEndpoint.RequestUploadFile, TResponseDomain, 
                RequestUploadFileMapper, ResponseJsonMapper<TResponseDomain, TResponseDto, TMapper>>
                where TMapper : IWebApiEndpointResponseDtoMapper<TResponseDomain, TResponseDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<TResponseDomain>> ExecuteAsync(WebApiEndpoint.RequestUploadFile request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response async enumerable
        /// </summary>
        public abstract class ResponseAsyncEnumerable<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseAsyncEnumerableDto<TDataDto>, WebApiEndpoint.RequestUploadFile, ResponseAsyncEnumerable<TData>, RequestUploadFileMapper, ResponseAsyncEnumerableMapper<TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseAsyncEnumerable<TData>>> ExecuteAsync(WebApiEndpoint.RequestUploadFile request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response bytes
        /// </summary>
        public abstract class ResponseBytes : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseBytesDto, WebApiEndpoint.RequestUploadFile, WebApiEndpoint.ResponseBytes, RequestUploadFileMapper, ResponseBytesMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseBytes>> ExecuteAsync(WebApiEndpoint.RequestUploadFile request, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response data collection
        /// </summary>
        public abstract class ResponseDataCollection<TDataDto, TData>
        {
            public abstract class Mapper<TResponseDataMapper> : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseDataCollectionDto<TDataDto>, WebApiEndpoint.RequestUploadFile, ResponseDataCollection<TData>, RequestUploadFileMapper, ResponseDataCollectionMapper<TData, TDataDto, TResponseDataMapper>>
                where TResponseDataMapper : IWebApiEndpointResponseDataMapper<TData, TDataDto>
            {
                /// <inheritdoc />
                public abstract Task<Result<ResponseDataCollection<TData>>> ExecuteAsync(WebApiEndpoint.RequestUploadFile request, CancellationToken cancellationToken);
            }
        }

        /// <summary>
        /// Configure with response empty json
        /// </summary>
        public abstract class ResponseEmptyJson : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseEmptyJsonDto, WebApiEndpoint.RequestUploadFile, WebApiEndpoint.ResponseEmptyJson, RequestUploadFileMapper, ResponseEmptyJsonMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseEmptyJson>> ExecuteAsync(WebApiEndpoint.RequestUploadFile request, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response file stream
        /// </summary>
        public abstract class ResponseFileStream : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseFileStreamDto, WebApiEndpoint.RequestUploadFile, WebApiEndpoint.ResponseFileStream, RequestUploadFileMapper, ResponseFileStreamMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseFileStream>> ExecuteAsync(WebApiEndpoint.RequestUploadFile request, CancellationToken cancellationToken);
        }

        /// <summary>
        /// Configure with response stream
        /// </summary>
        public abstract class ResponseStream : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseStreamDto, WebApiEndpoint.RequestUploadFile, WebApiEndpoint.ResponseStream, RequestUploadFileMapper, ResponseStreamMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseStream>> ExecuteAsync(WebApiEndpoint.RequestUploadFile request, CancellationToken cancellationToken);
        }
        
        /// <summary>
        /// Configure with response redirect
        /// </summary>
        public abstract class ResponseRedirect : ICommandWebApiEndpoint<RequestUploadFileDto, ResponseRedirectDto, WebApiEndpoint.RequestUploadFile, WebApiEndpoint.ResponseRedirect, RequestUploadFileMapper, ResponseRedirectMapper>
        {
            /// <inheritdoc />
            public abstract Task<Result<WebApiEndpoint.ResponseRedirect>> ExecuteAsync(WebApiEndpoint.RequestUploadFile request, CancellationToken cancellationToken);
        }
    }
}