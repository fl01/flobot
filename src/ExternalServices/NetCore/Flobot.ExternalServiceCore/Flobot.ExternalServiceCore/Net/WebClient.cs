using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flobot.ExternalServiceCore.Net
{
    public class WebClient
    {
        public async Task<ContentResponse> GetContentAsync(Uri url)
        {
            HttpWebRequest request = CreateWebRequest(url, HttpMethod.Get);

            var response = await request.GetResponseAsync();

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string content = streamReader.ReadToEnd();
                var result = new ContentResponse()
                {
                    Content = content,
                    ResponseUrl = response.ResponseUri
                };

                return result;
            }
        }

        public async Task<ContentResponse> GetContentAsync(string url)
        {
            return await GetContentAsync(new Uri(url));
        }

        private HttpWebRequest CreateWebRequest(Uri url, HttpMethod method)
        {
            var httpWebRequest = HttpWebRequest.CreateHttp(url);
            httpWebRequest.Method = method.Method;
            httpWebRequest.Headers["User-Agent"] = "Chrome";
            return httpWebRequest;
        }
    }
}
