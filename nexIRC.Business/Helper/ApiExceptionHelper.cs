using System.Net;

namespace nexIRC.Business.Helper {
    /// <summary>
    /// Api Exception Helper
    /// </summary>
    public class ApiExceptionHelper : Exception {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="requestContent"></param>
        /// <param name="responseContent"></param>
        /// <param name="statusCode"></param>
        public ApiExceptionHelper(Uri uri, string? requestContent, string? responseContent, HttpStatusCode statusCode) : base($"Matrix API error. Status: {statusCode}, json: {responseContent}") {
            Uri = uri;
            RequestContent = requestContent;
            ResponseContent = responseContent;
            StatusCode = statusCode;
        }
        /// <summary>
        /// Uri
        /// </summary>
        public Uri Uri { get; }
        /// <summary>
        /// Request Content
        /// </summary>
        public string? RequestContent { get; }
        /// <summary>
        /// Response Content
        /// </summary>
        public string? ResponseContent { get; }
        /// <summary>
        /// Status Code
        /// </summary>
        public HttpStatusCode StatusCode { get; }
    }
}