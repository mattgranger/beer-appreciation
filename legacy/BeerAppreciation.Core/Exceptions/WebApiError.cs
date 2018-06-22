namespace BeerAppreciation.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents an error that occurred during processing of a Web Api request
    /// </summary>
    public class WebApiError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiError"/> class.
        /// </summary>
        public WebApiError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiError" /> class. This overload will create a populate WebApiError instance
        /// with its state derived from the exception
        /// </summary>
        /// <param name="exception">The exception.</param>
        public WebApiError(Exception exception)
        {
            this.ParseException(exception);
        }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public int? ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the inner aggregateException message.
        /// </summary>
        public string InnerExceptionMessage { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the validation errors.
        /// </summary>
        public IList<ValidationError> ValidationErrors { get; set; }

        /// <summary>
        /// Parses the aggregateException to populate the WebApiError instance state
        /// </summary>
        /// <param name="exception">The aggregateException.</param>
        private void ParseException(Exception exception)
        {
            AggregateException aggregateException = exception as AggregateException;

            if (aggregateException != null)
            {
                // AggregateException, so make sure we include the InnerExceptions
                this.ParseAggregateException(aggregateException);
            }
            else
            {
                this.Message = exception.Message;
                this.StackTrace = exception.StackTrace;

                // Add the inner aggregateException
                if (exception.InnerException != null)
                {
                    this.InnerExceptionMessage = exception.InnerException.Message;
                    this.StackTrace = exception.StackTrace;
                }
            }
        }

        /// <summary>
        /// Parses the aggregate exception to populate the WebApiError instance state.
        /// </summary>
        /// <param name="aggregateException">The aggregate exception.</param>
        private void ParseAggregateException(AggregateException aggregateException)
        {
            if (aggregateException != null)
            {
                // Populate the outer exception details
                this.Message = aggregateException.Message;
                this.StackTrace = aggregateException.StackTrace;

                // Parse the inner exceptions
                if (aggregateException.InnerExceptions != null && aggregateException.InnerExceptions.Count > 1)
                {
                    StringBuilder innerExceptionMessage = new StringBuilder();
                    StringBuilder innerExceptionStackTrace = new StringBuilder();

                    int exceptionCount = 1;

                    foreach (Exception exception in aggregateException.InnerExceptions)
                    {
                        string message = string.Format("{0}) {1} ", exceptionCount, exception.Message);
                        innerExceptionMessage.Append(message);

                        string stackTrace = string.Format("{0}) {1} ", exceptionCount, exception.StackTrace);
                        innerExceptionStackTrace.Append(stackTrace);

                        exceptionCount++;
                    }

                    this.InnerExceptionMessage = innerExceptionMessage.ToString();
                    this.StackTrace += "\r\n" + innerExceptionStackTrace.ToString();
                }
            }
        }
    }
}
