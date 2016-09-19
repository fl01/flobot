using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Flobot.Common.Net
{
    public class HttpClient
    {
        public string PostJson(Uri url, object obj)
        {
            var request = CreateJsonWebRequest(url, WebRequestMethods.Http.Post);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string jsonObject = JsonConvert.SerializeObject(obj);
                streamWriter.Write(jsonObject);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public T GetJsonObject<T>(Uri url)
        {
            var httpWebRequest = CreateJsonWebRequest(url, WebRequestMethods.Http.Get);

            using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string rawJson = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(rawJson);
                }
            }
        }

        private HttpWebRequest CreateJsonWebRequest(Uri url, string method)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = method;
            return httpWebRequest;
        }

        public bool Ping(Uri url)
        {
            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            //request.AllowAutoRedirect = false;
            //request.Method = "HEAD";
            //try
            //{
            //    var response = request.GetResponse();
            //    return true;
            //}
            //catch (WebException ex)
            //{
            //    return false;
            //}
            return true;
        }
    }
}
