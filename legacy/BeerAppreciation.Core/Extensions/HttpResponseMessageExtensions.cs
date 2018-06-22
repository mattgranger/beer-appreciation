namespace BeerAppreciation.Core.Extensions
{
    using Serialisation;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Formatting;

    /// <summary>
    /// Extension methods for the HttpResponse/HttpRequestMessages
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        #region Fields and Constants

        /// <summary>
        /// A list of media type Formatters to use for de-serialisation
        /// </summary>
        private static readonly IList<MediaTypeFormatter> Formatters = new List<MediaTypeFormatter> 
        { 
            new ExtendedXmlMediaTypeFormatter(),
            new JsonMediaTypeFormatter(),
        };

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserialises HttpResponseMessage content
        /// </summary>
        /// <typeparam name="T">The type into which the response is deserialised</typeparam>
        /// <param name="response">The response.</param>
        /// <returns>The deserialised response</returns>
        public static T Deserialize<T>(this HttpResponseMessage response)
        {
            T result = default(T);

            if (response != null && response.Content != null)
            {
                result = response.Content.ReadAsAsync<T>(Formatters).Result;
            }

            return result;
        }

        #endregion
    }

}
