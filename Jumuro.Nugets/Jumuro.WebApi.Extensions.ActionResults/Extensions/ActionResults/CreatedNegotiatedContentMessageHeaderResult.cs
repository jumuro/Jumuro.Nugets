using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Jumuro.WebApi.Extensions.ActionResults
{
    /// <summary>
    /// Represents an action result that performs content negotiation and returns a <see cref="F:System.Net.HttpStatusCode.Created"/> response and a "Message" header when it succeeds.
    /// </summary>
    /// <typeparam name="T">The type of content in the entity body.</typeparam>
    public class CreatedNegotiatedContentMessageHeaderResult<T> : CreatedNegotiatedContentResult<T>
    {
        private readonly string _message;

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
        /// Initializes a new instance of the <see cref="T:Jumuro.WebApi.Extensions.ActionResults.CreatedNegotiatedContentMessageHeaderResult"/> class with the values provided.
        /// </summary>
        /// <param name="location">The content value to negotiate and format in the entity body.</param>
        /// <param name="content">The location at which the content has been created.</param>
        /// <param name="message">The message to include in the "Message" header.</param>
        /// <param name="contentNegotiator">The content negotiator to handle content negotiation.</param>
        /// <param name="request">The request message which led to this result.</param>
        /// <param name="formatters">The formatters to use to negotiate and format the content.</param>
        public CreatedNegotiatedContentMessageHeaderResult(Uri location, T content, string message, IContentNegotiator contentNegotiator, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
            : base(location, content, contentNegotiator, request, formatters)
        {
            this._message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Jumuro.WebApi.Extensions.ActionResults.CreatedNegotiatedContentMessageHeaderResult"/> class with the values provided.
        /// </summary>
        /// <param name="location">The location at which the content has been created.</param>
        /// <param name="content">The content value to negotiate and format in the entity body.</param>
        /// <param name="message">The message to include in the "Message" header.</param>
        /// <param name="controller">The controller from which to obtain the dependencies needed for execution.</param>
        public CreatedNegotiatedContentMessageHeaderResult(Uri location, T content, string message, ApiController controller)
            : base(location, content, controller)
        {
            this._message = message;
        }

        /// <summary>
        /// Executes asynchronously the operation of the created negotiated content result.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = base.ExecuteAsync(cancellationToken).Result;

            response.Content.Headers.Add("Message", Message);

            return Task.FromResult(response);
        }
    }
}
