using Jumuro.WebApi.Extensions.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http.Formatting;

namespace Jumuro.WebApi.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApiControllerExtensions
    {
        #region NotFound

        /// <summary>Creates a <see cref="NotFoundMessageHeaderResult"/> (404 Not Found) with a "Message" header.</summary>
        /// <param name="controller">The current ApiController.</param>
        /// <param name="message">The message to include in the "Message" header.</param>
        /// <returns>A <see cref="NotFoundMessageHeaderResult"/>.</returns>
        public static NotFoundMessageHeaderResult NotFound(this ApiController controller, string message)
        {
            return new NotFoundMessageHeaderResult(message, controller);
        } 

        #endregion

        #region Created

        /// <summary>
        /// Creates a <see cref="CreatedNegotiatedContentMessageHeaderResult{T}"/> (201 Created) with a "Message" header and with the specified values.
        /// </summary>
        /// <typeparam name="T">The type of content in the entity body.</typeparam>
        /// <param name="controller">The current ApiController.</param>
        /// <param name="location">
        /// The location at which the content has been created. Must be a relative or absolute URL.
        /// </param>
        /// <param name="content">The content value to negotiate and format in the entity body.</param>
        /// <param name="message">The message to include in the "Message" header.</param>
        /// <returns>A <see cref="CreatedNegotiatedContentMessageHeaderResult{T}"/> with the specified values.</returns>
        public static CreatedNegotiatedContentMessageHeaderResult<T> Created<T>(this ApiController controller, string location, T content, string message)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location");
            }

            return Created<T>(controller, new Uri(location, UriKind.RelativeOrAbsolute), content, message);
        }

        /// <summary>
        /// Creates a <see cref="CreatedNegotiatedContentMessageHeaderResult{T}"/> (201 Created) with a "Message" header and with the specified values.
        /// </summary>
        /// <typeparam name="T">The type of content in the entity body.</typeparam>
        /// <param name="controller">The current ApiController.</param>
        /// <param name="location">The location at which the content has been created.</param>
        /// <param name="content">The content value to negotiate and format in the entity body.</param>
        /// <param name="message">The message to include in the "Message" header.</param>
        /// <returns>A <see cref="CreatedNegotiatedContentMessageHeaderResult{T}"/> with the specified values.</returns>
        public static CreatedNegotiatedContentMessageHeaderResult<T> Created<T>(this ApiController controller, Uri location, T content, string message)
        {
            return new CreatedNegotiatedContentMessageHeaderResult<T>(location, content, message, controller);
        }

        #endregion

        #region Ok

        /// <summary>
        /// Creates an <see cref="OkNegotiatedContentMessageHeaderResult{T}"/> (200 OK) with a "Message" header and with the specified values.
        /// </summary>
        /// <typeparam name="T">The type of content in the entity body.</typeparam>
        /// <param name="controller">The current ApiController.</param>
        /// <param name="content">The content value to negotiate and format in the entity body.</param>
        /// <param name="message">The message to include in the "Message" header.</param>
        /// <returns>An <see cref="OkNegotiatedContentMessageHeaderResult{T}"/> with the specified values.</returns>
        public static OkNegotiatedContentMessageHeaderResult<T> Ok<T>(this ApiController controller, T content, string message)
        {
            return new OkNegotiatedContentMessageHeaderResult<T>(content, message, controller);
        }

        /// <summary>
        /// Creates an <see cref="OkNegotiatedContentPaginationHeadersResult{T}"/> (200 OK) with a "Message" header and with the specified values.
        /// </summary>
        /// <typeparam name="T">The type of content in the entity body.</typeparam>
        /// <param name="controller">The current ApiController.</param>
        /// <param name="content">The content value to negotiate and format in the entity body.</param>
        /// <param name="currentPage">The value to include in the "X-Pagination-Current-Page" header.</param>
        /// <param name="perPage">The value to include in the "X-Pagination-Per-Page" header.</param>
        /// <param name="totalPages">The value to include in the "X-Pagination-Total-Pages" header.</param>
        /// <param name="totalEntries">The value to include in the "X-Pagination-Total-Entries" header.</param>
        /// <param name="currentPageParamName">The name of the pagination querystring param to indicate the current page.</param>
        /// <param name="perPageParamName">The name of the pagination querystring param to indicate the per page entries.</param>
        /// <returns>An <see cref="OkNegotiatedContentPaginationHeadersResult{T}"/> with the specified values.</returns>
        public static OkNegotiatedContentPaginationHeadersResult<T> Ok<T>(this ApiController controller, T content, int currentPage, int perPage, int totalPages, int totalEntries, string currentPageParamName, string perPageParamName)
        {
            return new OkNegotiatedContentPaginationHeadersResult<T>(content, currentPage, perPage, totalPages, totalEntries, currentPageParamName, perPageParamName, controller);
        }

        #endregion

        #region Conflict

        /// <summary>Creates a <see cref="ConflictMessageHeaderResult"/> (409 Conflict) with a "Message" header.</summary>
        /// <param name="controller">The current ApiController.</param>
        /// <param name="message">The message to include in the "Message" header.</param>
        /// <returns>A <see cref="ConflictMessageHeaderResult"/>.</returns>
        public static ConflictMessageHeaderResult Conflict(this ApiController controller, string message)
        {
            return new ConflictMessageHeaderResult(message, controller);
        }

        #endregion
    }
}