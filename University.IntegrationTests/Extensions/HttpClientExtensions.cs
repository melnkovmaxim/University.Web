using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace University.IntegrationTests.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<string> GetResponseFromRequest(this HttpClient client, HttpMethod method, string uri, Dictionary<string, string> dictionary = default)
        {
            var postRequest = new HttpRequestMessage(method, uri);

            if (dictionary != default)
            {
                postRequest.Content = new FormUrlEncodedContent(dictionary);
            }

            var response = await client.SendAsync(postRequest);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
