﻿using Jumuro.WebApi.Extensions.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http.Formatting;

namespace Jumuro.WebApi.Extensions
{
    public static class ApiControllerExtensions
    {
        #region NotFound
        /// <summary>
        /// NotFound
        /// </summary>
        /// <param name="oController"></param>
        /// <param name="sMessage"></param>
        /// <returns></returns>
        public static NotFoundTextPlainActionResult NotFound(this ApiController oController, string sMessage)
        {
            return new NotFoundTextPlainActionResult(sMessage, oController.Request);
        } 
        #endregion

        #region Created
        /// <summary>
        /// Created
        /// </summary>
        /// <param name="oController"></param>
        /// <param name="sMessage"></param>
        /// <returns></returns>
        public static CreatedNoLocationResult<T> Created<T>(this ApiController oController, T oContent, string sMessage)
        {
            IContentNegotiator negotiator = oController.Configuration.Services.GetContentNegotiator();
            ContentNegotiationResult result = negotiator.Negotiate(typeof(T), oController.Request, oController.Configuration.Formatters);
            var bestMatchFormatter = result.Formatter;
            var mediaType = result.MediaType.MediaType;

            return new CreatedNoLocationResult<T>(oContent, sMessage, oController.Request, bestMatchFormatter, mediaType);
        }
        #endregion

        #region Ok
        ///// <summary>
        ///// Ok
        ///// </summary>
        ///// <param name="oController"></param>
        ///// <param name="sMessage"></param>
        ///// <returns></returns>
        //public static OkMessageResult<T> Ok<T>(this ApiController oController, T oContent, string sMessage)
        //{
        //    return new OkMessageResult<T>(oContent, sMessage, oController.Request);
        //}

        /// <summary>
        /// Ok
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static OkNegotiatedContentMessageHeaderResult<T> Ok<T>(this ApiController controller, T content, string message)
        {
            return new OkNegotiatedContentMessageHeaderResult<T>(content, message, controller);
        }

        /// <summary>
        /// Ok
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="content"></param>
        /// <param name="currentPage"></param>
        /// <param name="perPage"></param>
        /// <param name="totalPages"></param>
        /// <param name="totalEntries"></param>
        /// <param name="currentPageParamName"></param>
        /// <param name="perPageParamName"></param>
        /// <returns></returns>
        public static OkNegotiatedContentPaginatioHeadersResult<T> Ok<T>(this ApiController controller, T content, int currentPage, int perPage, int totalPages, int totalEntries, string currentPageParamName, string perPageParamName)
        {
            return new OkNegotiatedContentPaginatioHeadersResult<T>(content, currentPage, perPage, totalPages, totalEntries, currentPageParamName, perPageParamName, controller);
        }

        #endregion

        #region Conflict
        /// <summary>
        /// NotFound
        /// </summary>
        /// <param name="oController"></param>
        /// <param name="sMessage"></param>
        /// <returns></returns>
        public static ConflictTextPlainActionResult Conflict(this ApiController oController, string sMessage)
        {
            return new ConflictTextPlainActionResult(sMessage, oController.Request);
        }
        #endregion

    }
}