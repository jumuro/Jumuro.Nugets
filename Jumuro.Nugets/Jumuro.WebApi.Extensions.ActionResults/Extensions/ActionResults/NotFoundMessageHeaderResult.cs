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
    /// Represents an action result that returns an empty <see cref="F:System.Net.HttpStatusCode.NotFound"/> response and a "Message" header.
    /// </summary>
    public class NotFoundMessageHeaderResult : NotFoundResult
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
        /// Initializes a new instance of the <see cref="T:Jumuro.WebApi.Extensions.ActionResults.NotFoundMessageHeaderResult"/> class with the values provided.
        /// </summary>
        /// <param name="message">The message to include in the "Message" header.</param>
        /// <param name="request">The request message which led to this result.</param>
        public NotFoundMessageHeaderResult(string message, HttpRequestMessage request)
            : base(request)
        {
            this._message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Jumuro.WebApi.Extensions.ActionResults.NotFoundMessageHeaderResult"/> class with the values provided.
        /// </summary>
        /// <param name="message">The message to include in the "Message" header.</param>
        /// <param name="controller">The controller from which to obtain the dependencies needed for execution.</param>
        public NotFoundMessageHeaderResult(string message, ApiController controller)
            : base(controller)
        {
            this._message = message;
        }

        /// <summary>
        /// Executes asynchronously the operation of the ok negotiated content result.
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
