using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Jumuro.WebApi.Extensions.ActionResults
{
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
}
