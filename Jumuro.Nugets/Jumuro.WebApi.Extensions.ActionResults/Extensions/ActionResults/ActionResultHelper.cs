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

    #region CreatedNoLocationResult
    /// <summary>
    /// CreatedNoLocationResult
    /// </summary>
    public class CreatedNoLocationResult<T>:  IHttpActionResult
    {
        public CreatedNoLocationResult(T oContent, string sMessage, HttpRequestMessage oRequest, MediaTypeFormatter oMediaTypeFormatter, string sMediaType)
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
            Message = sMessage;
            Request = oRequest;
            RequestMediaType = sMediaType;
            RequestMediaTypeFormatter = oMediaTypeFormatter;
        }

        public string Message { get; private set; }
        public T Content { get; private set; }
        public string RequestMediaType { get; private set; }
        public MediaTypeFormatter RequestMediaTypeFormatter { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
            //var _objectContent = (T)Content;



            response.Content = new ObjectContent<T>(Content, RequestMediaTypeFormatter, RequestMediaType); // Put the message in the response body (text/plain content).
            response.Content.Headers.Add("Message", Message);
            response.RequestMessage = Request;
            return response;
        }
    }
    #endregion

    //#region OkMessageResult
    ///// <summary>
    ///// OkMessageResult
    ///// </summary>
    //public class OkMessageResult<T> : IHttpActionResult
    //{
    //    public OkMessageResult(T content, string message, ApiController controller)
    //    {
    //        if (oContent == null)
    //        {
    //            throw new ArgumentNullException("message");
    //        }

    //        if (oRequest == null)
    //        {
    //            throw new ArgumentNullException("request");
    //        }

    //        Content = oContent;
    //        Message = sMessage;
    //        Request = oRequest;
    //    }

    //    public string Message { get; private set; }
    //    public T Content { get; private set; }

    //    public HttpRequestMessage Request { get; private set; }

    //    public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
    //    {
    //        return Task.FromResult(Execute());
    //    }

    //    public HttpResponseMessage Execute()
    //    {
    //        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
    //        //var _objectContent = (T)Content;
    //        response.Content = new ObjectContent<T>(Content, new JsonMediaTypeFormatter()); // Put the message in the response body (text/plain content).
    //        response.Content.Headers.Add("Message", Message);
    //        response.RequestMessage = Request;
    //        return response;
    //    }
    //}
    //#endregion

    #region OkNegotiatedContentMessageHeaderResult
    /// <summary>
    /// Represents an action result that performs content negotiation and returns an HttpStatusCode.OK response and a "Message" header when it succeeds.
    /// </summary>
    /// <typeparam name="T">The type of content in the entity body.</typeparam>
    public class OkNegotiatedContentMessageHeaderResult<T> : OkNegotiatedContentResult<T>
    {
        private readonly string _message;

        /// <summary>
        /// Gets the content value to negotiate and format in the entity body.
        /// </summary>
        public T Content
        {
            get
            {
                return base.Content;
            }
        }

        /// <summary>
        /// Gets the message to include in the "Message" header.
        /// </summary>
        public string Message
        {
            get
            {
                return this._message;
            }
        }

        /// <summary>
        /// Gets the content negotiator to handle content negotiation.
        /// </summary>
        public IContentNegotiator ContentNegotiator
        {
            get
            {
                return base.ContentNegotiator;
            }
        }

        /// <summary>
        /// Gets the request message which led to this result.
        /// </summary>
        public HttpRequestMessage Request
        {
            get
            {
                return base.Request;
            }
        }

        /// <summary>
        /// Gets the formatters to use to negotiate and format the content.
        /// </summary>
        public IEnumerable<MediaTypeFormatter> Formatters
        {
            get
            {
                return base.Formatters;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Espa.WebApi.Extensions.ActionResults.OkNegotiatedContentMessageHeaderResult"/> class with the values provided.
        /// </summary>
        /// <param name="content">The content value to negotiate and format in the entity body.</param>
        /// <param name="message">The message to include in the "Message" header.</param>
        /// <param name="controller">The controller from which to obtain the dependencies needed for execution.</param>
        public OkNegotiatedContentMessageHeaderResult(T content, string message, ApiController controller)
            : base(content, controller)
        {
            this._message = message;
        }

        public override Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = base.ExecuteAsync(cancellationToken).Result;

            response.Content.Headers.Add("Message", Message);

            return Task.FromResult(response);
        }
    }

    #endregion

    #region OkNegotiatedContentPaginatioHeadersResult
    /// <summary>
    /// Represents an action result that performs content negotiation and returns an HttpStatusCode.OK response and
    /// "X-Pagination-Current-Page", "X-Pagination-Per-Page", "X-Pagination-Total-Pages", "X-Pagination-Total-Entries" and "Link" headers when it succeeds.
    /// </summary>
    /// <typeparam name="T">The type of content in the entity body.</typeparam>
    public class OkNegotiatedContentPaginatioHeadersResult<T> : OkNegotiatedContentResult<T>
    {
        private readonly string _currentPageParamName;
        private readonly string _perPageParamName;
        private readonly int _currentPage;
        private readonly int _perPage;
        private readonly int _totalPages;
        private readonly int _totalEntries;

        /// <summary>
        /// Gets the value to include in the "X-Pagination-Current-Page" header.
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return this._currentPage;
            }
        }

        /// <summary>
        /// Gets the value to include in the "X-Pagination-Per-Page" header.
        /// </summary>
        public int PerPage
        {
            get
            {
                return this._perPage;
            }
        }

        /// <summary>
        /// Gets the value to include in the "X-Pagination-Total-Pages" header.
        /// </summary>
        public int TotalPages
        {
            get
            {
                return this._totalPages;
            }
        }

        /// <summary>
        /// Gets the value to include in the "X-Pagination-Total-Entries" header.
        /// </summary>
        public int TotalEntries
        {
            get
            {
                return this._totalEntries;
            }
        }


        /// <summary>
        /// Gets the name of the pagination querystring param to indicate the current page.
        /// </summary>
        public string CurrentPageParamName
        {
            get
            {
                return this._currentPageParamName;
            }
        }

        /// <summary>
        /// Gets the name of the pagination querystring param to indicate the per page entries.
        /// </summary>
        public string PerPageParamName
        {
            get
            {
                return this._perPageParamName;
            }
        }

        /// <summary>
        /// Gets the content value to negotiate and format in the entity body.
        /// </summary>
        public T Content
        {
            get
            {
                return base.Content;
            }
        }

        /// <summary>
        /// Gets the content negotiator to handle content negotiation.
        /// </summary>
        public IContentNegotiator ContentNegotiator
        {
            get
            {
                return base.ContentNegotiator;
            }
        }

        /// <summary>
        /// Gets the request message which led to this result.
        /// </summary>
        public HttpRequestMessage Request
        {
            get
            {
                return base.Request;
            }
        }

        /// <summary>
        /// Gets the formatters to use to negotiate and format the content.
        /// </summary>
        public IEnumerable<MediaTypeFormatter> Formatters
        {
            get
            {
                return base.Formatters;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Espa.WebApi.Extensions.ActionResults.OkNegotiatedContentPaginatioHeadersResult"/> class with the values provided.
        /// </summary>
        /// <param name="content">The content value to negotiate and format in the entity body.</param>
        /// <param name="message">The message to include in the "Message" header.</param>
        /// <param name="controller">The controller from which to obtain the dependencies needed for execution.</param>
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Espa.WebApi.Extensions.ActionResults.OkNegotiatedContentMessageHeaderResult"/> class with the values provided.
        /// </summary>
        /// <param name="content">The content value to negotiate and format in the entity body.</param>
        /// <param name="currentPage">The value to include in the "X-Pagination-Current-Page" header.</param>
        /// <param name="perPage">The value to include in the "X-Pagination-Per-Page" header.</param>
        /// <param name="totalPages">The value to include in the "X-Pagination-Total-Pages" header.</param>
        /// <param name="totalEntries">The value to include in the "X-Pagination-Total-Entries" header.</param>
        /// <param name="currentPageParamName">The name of the pagination querystring param to indicate the current page.</param>
        /// <param name="perPageParamName">The name of the pagination querystring param to indicate the per page entries.</param>
        /// <param name="controller">The controller from which to obtain the dependencies needed for execution.</param>
        public OkNegotiatedContentPaginatioHeadersResult(T content, int currentPage, int perPage, int totalPages, int totalEntries, string currentPageParamName, string perPageParamName, ApiController controller)
            : base(content, controller)
        {
            this._currentPage = currentPage;
            this._perPage = perPage;
            this._totalPages = totalPages;
            this._totalEntries = totalEntries;
            this._currentPageParamName = currentPageParamName;
            this._perPageParamName = perPageParamName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = base.ExecuteAsync(cancellationToken).Result;

            // Set related links
            var baseUrl = Request.RequestUri.AbsoluteUri.Substring(0, Request.RequestUri.AbsoluteUri.IndexOf(Request.RequestUri.Query));

            var firstLink = string.Format("{0}?{1}={2}&{3}={4}", baseUrl, _currentPageParamName, 1, _perPageParamName, _perPage);
            var prevLink = _currentPage > 1 ? string.Format("{0}?{1}={2}&{3}={4}", baseUrl, _currentPageParamName, _currentPage, _perPageParamName, _perPage) : "";
            var selfLink = Request.RequestUri.AbsoluteUri;
            var nextLink = _currentPage < _totalPages - 1 ? string.Format("{0}?{1}={2}&{3}={4}", baseUrl, _currentPageParamName, _currentPage + 1, _perPageParamName, _perPage) : "";
            var lastLink = string.Format("{0}?{1}={2}&{3}={4}", baseUrl, _currentPageParamName, _totalPages, _perPageParamName, _perPage);
            
            var links = new Dictionary<string, string>();
            links.Add("first", firstLink);
            links.Add("prev", prevLink);
            links.Add("self", selfLink);
            links.Add("next", nextLink);
            links.Add("last", lastLink);

            // Expose pagination headers in cors
            response.Headers.Add("Access-Control-Expose-Headers", new List<string> { "X-Pagination-Per-Page",
                                                                                     "X-Pagination-Current-Page",
                                                                                     "X-Pagination-Total-Pages",
                                                                                     "X-Pagination-Total-Entries",
                                                                                     "Link"
                                                                                   });

            // Add pagination headers
            response.Headers.Add("X-Pagination-Per-Page", _perPage.ToString());
            response.Headers.Add("X-Pagination-Current-Page", _currentPage.ToString());
            response.Headers.Add("X-Pagination-Total-Pages", _totalPages.ToString());
            response.Headers.Add("X-Pagination-Total-Entries", _totalEntries.ToString());
            response.Headers.Add("Link", String.Join(", ", links.Select(kvp => string.Format("<{0}>; rel=\"{1}\"", kvp.Value, kvp.Key))));

            return Task.FromResult(response);
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