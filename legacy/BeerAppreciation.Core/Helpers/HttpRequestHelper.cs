namespace BeerAppreciation.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Runtime.Serialization.Formatters;
    using System.Web.Http;
    using Exceptions;
    using Extensions;
    using Newtonsoft.Json;
    using Serialisation;

    /// <summary>
    /// Helper class allowing clients to make HTTP calls (GET, POST, PUT, DELETE)
    /// </summary>
    public static class HttpRequestHelper
    {
        #region Public Methods

        /// <summary>
        /// Performs a HTTP Get to the specified endpoint
        /// </summary>
        /// <param name="baseUri">The base uri E.g. http://localhost.</param>
        /// <param name="relativeUri">The relative endpoint.</param>
        /// <returns>
        /// The http response message
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "Want to call Get to fit in with the HTTP verb")]
        public static HttpResponseMessage Get(string baseUri, string relativeUri)
        {
            Uri absoluteUri = GetAbsoluteEndpoint(baseUri, relativeUri);

            return Get(absoluteUri, null);
        }

        /// <summary>
        /// Performs a HTTP Get to a specified endpoint with requires Basic authentication
        /// </summary>
        /// <param name="baseUri">The base uri E.g. http://localhost.</param>
        /// <param name="relativeUri">The relative endpoint.</param>
        /// <param name="credentials">The credentials.</param>
        /// <returns>
        /// The http response message
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "Want to call Get to fit in with the HTTP verb")]
        public static HttpResponseMessage Get(string baseUri, string relativeUri, NetworkCredential credentials)
        {
            Uri absoluteUri = GetAbsoluteEndpoint(baseUri, relativeUri);

            return Get(absoluteUri, credentials);
        }

        /// <summary>
        /// Performs a HTTP Get to the specified endpoint. This overload will de-serialize the response into type T.
        /// </summary>
        /// <typeparam name="T">The type that the content of the response will be de-serialised to</typeparam>
        /// <param name="baseUri">The base uri E.g. http://localhost</param>
        /// <param name="relativeUri">The relative endpoint.</param>
        /// <returns>
        /// The response of the GET de-serialized into type T
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "Want to call Get to fit in with the HTTP verb")]
        public static T Get<T>(string baseUri, string relativeUri)
        {
            // Get the absolute endpoint, adding any required oData parameters to query string
            Uri absoluteUri = GetAbsoluteEndpoint(baseUri, relativeUri);

            return Get<T>(absoluteUri, null);
        }

        /// <summary>
        /// Performs a HTTP Get to a specified endpoint that requires Basic authentication. This overload will de-serialize the response into type T.
        /// </summary>
        /// <typeparam name="T">The type that the content of the response will be de-serialised to</typeparam>
        /// <param name="baseUri">The base uri E.g. http://localhost</param>
        /// <param name="relativeUri">The relative endpoint.</param>
        /// <param name="credentials">The credentials if endpoint has basic authentication.</param>
        /// <returns>
        /// The response of the GET de-serialized into type T
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "Want to call Get to fit in with the HTTP verb")]
        public static T Get<T>(string baseUri, string relativeUri, NetworkCredential credentials)
        {
            // Get the absolute endpoint, adding any required oData parameters to query string
            Uri absoluteUri = GetAbsoluteEndpoint(baseUri, relativeUri);

            return Get<T>(absoluteUri, credentials);
        }

        /// <summary>
        /// Performs a HTTP Get to a specified endpoint and de-serialize the response into type T. This is the base method that all 'GET' overloads call.
        /// </summary>
        /// <typeparam name="T">The type that the content of the response will be de-serialised to</typeparam>
        /// <param name="absoluteUri">The absolute URI.</param>
        /// <param name="credentials">The credentials if endpoint has basic authentication.</param>
        /// <returns>
        /// The response of the GET de-serialized into type T
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "This is fine")]
        public static T Get<T>(Uri absoluteUri, NetworkCredential credentials)
        {
            T result = default(T);

            // Make the call and get the response
            HttpResponseMessage response = Get(absoluteUri, credentials);
            EnsureSuccessStatusCode(response);

            // Read the result. This will deserialise the incoming response content
            if (response.IsSuccessStatusCode && response.Content != null && response.Content.Headers.ContentType != null)
            {
                // Get the type of media and use the formatter to get the content out of response
                MediaTypeFormatter formatter = GetMediaTypeFormatter(response.Content.Headers.ContentType.MediaType, false);

                result = response.Content.ReadAsAsync<T>(new List<MediaTypeFormatter> { formatter }).Result;
            }

            // Return the response containing the de-serialised result
            return result;
        }

        /// <summary>
        /// Performs a HTTP Post to the specified endpoint
        /// </summary>
        /// <param name="baseUri">The base uri E.g. http://localhost.</param>
        /// <param name="relativeUri">The relative endpoint.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the post
        /// </returns>
        public static HttpResponseMessage Post(string baseUri, string relativeUri)
        {
            return Post(baseUri, relativeUri, (object)null);
        }

        /// <summary>
        /// Performs a HTTP Post to the specified endpoint. This overload allows the user to override the default request timeout
        /// </summary>
        /// <param name="baseUri">The base uri E.g. http://localhost.</param>
        /// <param name="relativeUri">The relative endpoint.</param>
        /// <param name="timeoutSeconds">The timeout for the http request in seconds</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the post
        /// </returns>
        public static HttpResponseMessage Post(string baseUri, string relativeUri, int timeoutSeconds)
        {
            return PostRequest(baseUri, relativeUri, (object)null, timeoutSeconds);
        }

        /// <summary>
        /// Performs a HTTP Post to the specified endpoint. This overload takes the content as a type of T, and serializes into the request.
        /// </summary>
        /// <typeparam name="T">The type that the content of the request</typeparam>
        /// <param name="baseUri">The base uri E.g. http://localhost.</param>
        /// <param name="relativeUri">The relative endpoint.</param>
        /// <param name="content">The content to post.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the post
        /// </returns>
        public static HttpResponseMessage Post<T>(string baseUri, string relativeUri, T content) where T : class
        {
            return PostRequest(baseUri, relativeUri, content, null);
        }

        /// <summary>
        /// Performs a HTTP Post to the specified endpoint. This overload allows the user to specify the context as Type T and override the default request timeout.
        /// </summary>
        /// <typeparam name="T">The type that the content of the request</typeparam>
        /// <param name="baseUri">The base uri E.g. http://localhost.</param>
        /// <param name="relativeUri">The relative endpoint.</param>
        /// <param name="content">The content to post.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the post
        /// </returns>
        public static HttpResponseMessage Post<T>(string baseUri, string relativeUri, T content, int timeoutSeconds) where T : class
        {
            return PostRequest(baseUri, relativeUri, content, timeoutSeconds);
        }

        /// <summary>
        /// Post a file stream to the specified endpoint
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <param name="fileStream">The stream containing the file to post.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the post
        /// </returns>
        public static HttpResponseMessage PostFile(string baseUri, string relativeUri, Stream fileStream)
        {
            return PostFile(baseUri, relativeUri, fileStream, null);
        }

        /// <summary>
        /// Post a file to the specified endpoint. This overload allows the user to override the default request timeout
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <param name="fileStream">The stream containing the file to post.</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns>
        /// The response of the POST
        /// </returns>
        public static HttpResponseMessage PostFile(string baseUri, string relativeUri, Stream fileStream, int? timeoutSeconds)
        {
            HttpResponseMessage response = null;

            // Get the absolute endpoint
            Uri absoluteUri = GetAbsoluteEndpoint(baseUri, relativeUri);

            using (HttpClient httpClient = new HttpClient())
            {
                if (timeoutSeconds.HasValue)
                {
                    httpClient.Timeout = new TimeSpan(0, 0, timeoutSeconds.Value);
                }

                // Create the POST request
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, absoluteUri);
                request.Content = new StreamContent(fileStream);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Make the request
                response = httpClient.SendAsync(request).Result;

                // Ensure a successful response
                EnsureSuccessStatusCode(response);
            }

            return response;
        }

        /// <summary>
        /// Makes a http request to the specified endpoint to get a file
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>
        /// The response of the GET, contains the File
        /// </returns>
        public static Stream GetFile(string baseUri, string relativeUri)
        {
            Stream content = null;

            // Get the absolute endpoint
            Uri absoluteUri = GetAbsoluteEndpoint(baseUri, relativeUri);

            using (HttpClient httpClient = new HttpClient())
            {
                // Create the GET request
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, absoluteUri);
                request.Headers.Add("accept", "application/octet-stream");

                // Make the request 
                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Read the response
                    content = response.Content.ReadAsStreamAsync().Result;
                }
            }

            return content;
        }

        /// <summary>
        /// Performs a HTTP Put to the specified endpoint
        /// </summary>
        /// <param name="baseUri">The base uri E.g. http://localhost.</param>
        /// <param name="relativeUri">The relative endpoint.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the put
        /// </returns>
        public static HttpResponseMessage Put(string baseUri, string relativeUri)
        {
            return Put(baseUri, relativeUri, (object)null);
        }

        /// <summary>
        /// Performs a HTTP Put to the specified endpoint. This overload allows the content of type T to be specified to serialize into request.
        /// </summary>
        /// <typeparam name="T">The type that the content of the request</typeparam>
        /// <param name="baseUri">The base uri E.g. http://localhost.</param>
        /// <param name="relativeUri">The relative endpoint.</param>
        /// <param name="content">The content to put.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the put
        /// </returns>
        public static HttpResponseMessage Put<T>(string baseUri, string relativeUri, T content)
        {
            return Put(baseUri, relativeUri, content, false);
        }

        /// <summary>
        /// Performs a HTTP Put to the specified endpoint
        /// </summary>
        /// <typeparam name="T">The type that the content of the request</typeparam>
        /// <param name="baseUri">The base uri E.g. http://localhost.</param>
        /// <param name="relativeUri">The relative endpoint.</param>
        /// <param name="content">The content to put.</param>
        /// <param name="serializeTypeNames">if set to <c>true</c> serialize the type names sent in the request.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the put
        /// </returns>
        public static HttpResponseMessage Put<T>(string baseUri, string relativeUri, T content, bool serializeTypeNames)
        {
            HttpResponseMessage response = null;

            // Get the absolute endpoint, adding any required oData parameters to query string
            Uri absoluteUri = GetAbsoluteEndpoint(baseUri, relativeUri);

            using (HttpClient httpClient = new HttpClient())
            {
                // TODO: this is a Temp setting. will be deleted after testing
                httpClient.Timeout = new TimeSpan(1, 0, 0);

                // Create the PUT request
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, absoluteUri);

                // if content is null, then send a PUT request with no content.
                if (content == null)
                {
                    request.Content = new StringContent(string.Empty);
                }
                else
                {
                    string mediaType = string.Empty;

                    if (request.Headers.Accept != null && request.Headers.Accept.Count > 0)
                    {
                        mediaType = request.Headers.Accept.FirstOrDefault().MediaType;
                    }

                    // Serialise the content and add to the request
                    request.Content = new ObjectContent<T>(content, GetMediaTypeFormatter(mediaType, serializeTypeNames));
                }

                // Perform the PUT - HttpCompletionOption.ResponseHeadersRead is used to avoid memory leak
                response = httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;

                // Throw an exception if response has a non-success status code
                EnsureSuccessStatusCode(response);
            }

            return response;
        }

        /// <summary>
        /// Performs a HTTP Delete to the specified endpoint.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the delete.
        /// </returns>
        public static HttpResponseMessage Delete(string baseUri, string relativeUri)
        {
            return Delete(baseUri, relativeUri, (object)null);
        }

        /// <summary>
        /// Performs a HTTP Delete to the specified endpoint.
        /// </summary>
        /// <typeparam name="T">The content of the request message</typeparam>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <param name="content">The content of the message.</param>
        /// <returns>
        /// A HttpResponseMessage containing the result of the delete.
        /// </returns>
        public static HttpResponseMessage Delete<T>(string baseUri, string relativeUri, T content) where T : class
        {
            Uri absoluteUri = GetAbsoluteEndpoint(baseUri, relativeUri);

            using (HttpClient httpClient = new HttpClient())
            {
                // Create the request for the delete
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, absoluteUri);

                // if content is null, then send the request with no content.
                if (content == null)
                {
                    request.Content = new StringContent(string.Empty);
                }
                else
                {
                    string mediaType = string.Empty;

                    if (request.Headers.Accept != null && request.Headers.Accept.Count > 0)
                    {
                        mediaType = request.Headers.Accept.FirstOrDefault().MediaType;
                    }

                    // Serialise the content and add to the request
                    request.Content = new ObjectContent<T>(content, GetMediaTypeFormatter(mediaType, false));
                }

                // Make the request and get the response. - HttpCompletionOption.ResponseHeadersRead is used to avoid memory leak
                HttpResponseMessage response = httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;
                EnsureSuccessStatusCode(response);

                return response;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the media type formatter depending on the media
        /// </summary>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="serializeTypeNames">if set to <c>true</c> serialize the type names sent in the request.</param>
        /// <returns>
        /// The object content
        /// </returns>
        private static MediaTypeFormatter GetMediaTypeFormatter(string mediaType, bool serializeTypeNames)
        {
            JsonMediaTypeFormatter jsonMediaTypeFormatter = null;
            MediaTypeFormatter mediaTypeFormatter = null;

            switch (mediaType)
            {
                case "application/json":
                    jsonMediaTypeFormatter = new JsonMediaTypeFormatter();
                    if (serializeTypeNames)
                    {
                        jsonMediaTypeFormatter.SerializerSettings.TypeNameHandling = TypeNameHandling.All;
                        jsonMediaTypeFormatter.SerializerSettings.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
                    }

                    mediaTypeFormatter = jsonMediaTypeFormatter;
                    break;

                case "application/xml":
                case "text/xml":
                    mediaTypeFormatter = new ExtendedXmlMediaTypeFormatter();
                    break;

                default:
                    // Default to json
                    jsonMediaTypeFormatter = new JsonMediaTypeFormatter();
                    if (serializeTypeNames)
                    {
                        jsonMediaTypeFormatter.SerializerSettings.TypeNameHandling = TypeNameHandling.All;
                        jsonMediaTypeFormatter.SerializerSettings.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
                    }

                    mediaTypeFormatter = jsonMediaTypeFormatter;
                    break;
            }

            return mediaTypeFormatter;
        }

        /// <summary>
        /// Ensures the success status code in response, if not will throw a HttpResponseException
        /// </summary>
        /// <param name="response">The response.</param>
        private static void EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    // Add the content to the exception
                    throw new HttpResponseMessageException(response.Content.ReadAsStringAsync().Result, response);
                }

                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Appends the real live endpoint with the absolute and appends any OData parameters to query string
        /// </summary>
        /// <param name="baseUri">The base URi.</param>
        /// <param name="relativeUri">The relative URi.</param>
        /// <returns>
        /// The absolute endpoint
        /// </returns>
        private static Uri GetAbsoluteEndpoint(
            string baseUri,
            string relativeUri)
        {
            // Construct the absolute end point
            if (!string.IsNullOrEmpty(relativeUri) && relativeUri.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                // Remove the preceding backslash
                relativeUri = relativeUri.Substring(1);
            }

            if (!string.IsNullOrEmpty(baseUri) && baseUri.Trim().EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                // Remove the trailing backslash
                baseUri = baseUri.Substring(0, baseUri.Trim().Length - 1);
            }

            return new Uri(string.Format("{0}/{1}", baseUri, relativeUri));
        }

        /// <summary>
        /// Performs a HTTP Get to the specified endpoint
        /// </summary>
        /// <param name="absoluteUri">The absolute URI.</param>
        /// <param name="credentials">The credentials if endpoint has basic authentication.</param>
        /// <returns>
        /// An HttpResponseMessage containing de-serialised response data
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "Want to call Get to fit in with the HTTP verb")]
        private static HttpResponseMessage Get(Uri absoluteUri, NetworkCredential credentials)
        {
            // Removed the using statment for httpclient to pass integration test - need to keep an eye on it though
            HttpClient httpClient = GetHttpClient(absoluteUri, credentials);

            // Create the request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, absoluteUri);
            request.Headers.Add("Accept", "application/json");

            // Make the request  - HttpCompletionOption.ResponseHeadersRead is used to avoid memory leak
            return httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;
        }

        /// <summary>
        /// Gets the HTTP client with network credentials attached if necessary.
        /// </summary>
        /// <param name="absoluteUri">The absolute URI.</param>
        /// <param name="credentials">The network credential.</param>
        /// <returns>An instance of HttpClient</returns>
        private static HttpClient GetHttpClient(Uri absoluteUri, NetworkCredential credentials)
        {
            HttpClient httpClient = null;

            if (credentials != null)
            {
                CredentialCache credentialCache = new CredentialCache { { absoluteUri, "Basic", credentials } };

                httpClient = new HttpClient(new HttpClientHandler { Credentials = credentialCache, AllowAutoRedirect = false });
            }
            else
            {
                httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });
            }

            return httpClient;
        }

        /// <summary>
        /// Performs all Http post actions encapsualted into a single private method
        /// </summary>
        /// <typeparam name="T">Generic type to serialize in the post</typeparam>
        /// <param name="baseUri">The base uri</param>
        /// <param name="relativeUri">The relative uri</param>
        /// <param name="content">The content to post</param>
        /// <param name="timeoutSeconds">The timeout for the request in seconds</param>
        /// <returns>
        /// An HttpResponseMessage containing the result of the put
        /// </returns>
        private static HttpResponseMessage PostRequest<T>(string baseUri, string relativeUri, T content, int? timeoutSeconds) where T : class
        {
            HttpResponseMessage response = null;

            // Get the absolute endpoint, adding any required oData parameters to query string
            Uri absoluteUri = GetAbsoluteEndpoint(baseUri, relativeUri);

            using (HttpClient httpClient = new HttpClient())
            {
                if (timeoutSeconds.HasValue)
                {
                    httpClient.Timeout = new TimeSpan(0, 0, timeoutSeconds.Value);
                }

                // Create the POST request
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, absoluteUri);

                // if content is null, then send a POST request with no content.
                if (content == null)
                {
                    request.Content = new StringContent(string.Empty);
                }
                else
                {
                    string mediaType = string.Empty;

                    if (request.Headers.Accept != null && request.Headers.Accept.Count > 0)
                    {
                        mediaType = request.Headers.Accept.FirstOrDefault().MediaType;
                    }

                    // Serialise the content and add to the request
                    request.Content = new ObjectContent<T>(content, GetMediaTypeFormatter(mediaType, false));
                }

                // Perform the POST - HttpCompletionOption.ResponseHeadersRead is used to avoid memory leak
                response = httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;

                // Throw an exception if response has a non-success status code
                EnsureSuccessStatusCode(response);
            }

            return response;
        }

        #endregion
    }
}