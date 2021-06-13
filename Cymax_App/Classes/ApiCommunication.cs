using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TaylorFerens_Cymax_Application.Classes
{
    public static class ApiCommunication
    {
        #region Constants

        private const string ERROR_MAKE_HTTP_REQUEST_ERROR = "ERROR: MakeHttpRequest(): an error occured while making an http request. ";
        private const string ERROR_SEND_HTTP_REQUEST_ERROR = "ERROR: SendHttpRequest(): an error occured while making an http request. ";

        #endregion
        #region Public Static Methods

        public static HttpRequestMessage MakeHttpRequest(string shippingContent, string uri, string mediaType, string apiKey)
        {
            HttpRequestMessage request = null;

            try
            {
                if (!String.IsNullOrEmpty(shippingContent))
                {
                    request = new HttpRequestMessage(HttpMethod.Get, uri);

                    // Append the api key or "credentials"
                    request.Headers.Add("ApiKey", apiKey);

                    request.Content = new StringContent(shippingContent, Encoding.UTF8, mediaType);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ERROR_MAKE_HTTP_REQUEST_ERROR + ex);
            }

            return request;
        }

        public static async Task<string> SendHttpRequest(HttpRequestMessage httpRequest)
        {
            string opResult = null;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;

            try
            {
                if (httpRequest != null)
                {
                    response = await client.SendAsync(httpRequest);

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        opResult = response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ERROR_SEND_HTTP_REQUEST_ERROR + ex);
            }
            finally
            {
                client.Dispose();
                httpRequest.Dispose();
            }

            return opResult;
        }
        #endregion
    }
}
