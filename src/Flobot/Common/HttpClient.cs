using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Flobot.Common
{
    public class HttpClient
    {
        public string PostJson(Uri url,object obj)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string jsonObject = JsonConvert.SerializeObject(obj);
                streamWriter.Write(jsonObject);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
               return streamReader.ReadToEnd();
            }
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
