using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace BeerAppreciation.Core.WebApi.DelegatingHandlers
{
    /// <summary>
    /// Delegating handler that logs the request and response
    /// </summary>
    public class RequestResponseLoggingDelegatingHandler : DelegatingHandler
    {
        #region Fields and Constants

        /// <summary>
        /// The log4net logger
        /// </summary>
        private readonly ILogger log;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResponseLoggingDelegatingHandler"/> class.
        /// </summary>
        /// <param name="log">The castle windsor logging facility.</param>
        public RequestResponseLoggingDelegatingHandler(ILogger log)
        {
            this.log = log;
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Triggered before a WebApi request gets processed
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task for the async response</returns>
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Make the request
            return await base.SendAsync(request, cancellationToken).ContinueWith(
                task =>
                {
                    HttpResponseMessage response = task.Result;

                    // Log the request and response together so they appear as a pair in the log
                    this.LogRequestAndResponse(request, response);

                    return response;
                },
                cancellationToken);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Logs the request and response as a pair
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        private void LogRequestAndResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            // Construct the log message
            var builder = new StringBuilder(Environment.NewLine + "------------------------BEGIN REQUEST------------------------" + Environment.NewLine);

            // Log the entire request
            builder.AppendLine("REQUEST");
            builder.AppendLine("-------");
            builder.AppendLine(request.ToString());
            builder.AppendLine(request.Content != null ? request.Content.ReadAsStringAsync().Result : "<No Content>");
            builder.AppendLine();
            builder.AppendLine("RESPONSE");
            builder.AppendLine("--------");
            builder.AppendLine(response.ToString());
            builder.AppendLine(response.Content != null ? response.Content.ReadAsStringAsync().Result : "<No Content>");
            builder.AppendLine("------------------------END RESPONSE------------------------");

            this.log.Info(builder.ToString());
        }

        #endregion
    }
}
