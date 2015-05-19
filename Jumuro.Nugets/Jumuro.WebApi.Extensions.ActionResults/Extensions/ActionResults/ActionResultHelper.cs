using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;


namespace Jumuro.WebApi.Extensions.ActionResults
{
    #region NotFoundTextPlainActionResult
    /// <summary>
    /// NotFoundTextPlainActionResult
    /// </summary>
    public class NotFoundTextPlainActionResult : IHttpActionResult
    {
        public NotFoundTextPlainActionResult(string sMessage, HttpRequestMessage oRequest)
        {
            if (sMessage == null)
            {
                throw new ArgumentNullException("message");
            }

            if (oRequest == null)
            {
                throw new ArgumentNullException("request");
            }

            Message = sMessage;
            Request = oRequest;
        }

        public string Message { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
            response.Content = new StringContent(Message); // Put the message in the response body (text/plain content).
            response.RequestMessage = Request;
            return response;
        }
    } 
    #endregion

    #region OkFileResult
    /// <summary>
    /// OkFileResult
    /// </summary>
    public class OkFileResult : IHttpActionResult
    {
        public OkFileResult(byte[] oContent, HttpRequestMessage oRequest, string contentType)
        {
            if (oContent == null)
            {
                throw new ArgumentNullException("message");
            }

            if (oRequest == null)
            {
                throw new ArgumentNullException("request");
            }

            Content = oContent;
            Request = oRequest;
            ContentType = contentType;
        }

        public string Message { get; private set; }
        public byte[] Content { get; private set; }
        public string ContentType { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(Content);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
            response.RequestMessage = Request;
            return response;
        }
    }
    #endregion

    #region ConflictTextPlainActionResult
    /// <summary>
    /// NotFoundTextPlainActionResult
    /// </summary>
    public class ConflictTextPlainActionResult : IHttpActionResult
    {
        public ConflictTextPlainActionResult(string sMessage, HttpRequestMessage oRequest)
        {
            if (sMessage == null)
            {
                throw new ArgumentNullException("message");
            }

            if (oRequest == null)
            {
                throw new ArgumentNullException("request");
            }

            Message = sMessage;
            Request = oRequest;
        }

        public string Message { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Conflict);
            response.Content = new StringContent(Message); // Put the message in the response body (text/plain content).
            //response.Content.Headers.Add("Message", Message);
            response.RequestMessage = Request;
            return response;
        }
    }
    #endregion

    #region ErrorResult

    public class ErrorMessageResult : IHttpActionResult
    {
        private HttpResponseMessage _httpResponseMessage;


        public ErrorMessageResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
        {
            _httpResponseMessage = httpResponseMessage;
            _httpResponseMessage.RequestMessage = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_httpResponseMessage);
        }
    }



    #endregion

    #region ModelStateResult


    public class ModelStateErrorResult : IHttpActionResult
    {
        private readonly IEnumerable<string> _errorMessages;
        private readonly HttpRequestMessage _message;


        public ModelStateErrorResult(IEnumerable<string> errors, HttpRequestMessage message)
        {
            _errorMessages = errors;
            _message = message;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = new ObjectContent<IEnumerable<string>>(_errorMessages, new JsonMediaTypeFormatter()); ;
            response.RequestMessage = _message;
            return response;

        }
    }



    #endregion
}