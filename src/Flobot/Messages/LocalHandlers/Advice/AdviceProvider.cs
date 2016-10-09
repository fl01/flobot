using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Common.Net;
using Newtonsoft.Json;

namespace Flobot.Messages.LocalHandlers.Advice
{
    public class AdviceProvider
    {
        public string GetAdvice()
        {
            try
            {
                var httpClient = IoC.Container.Resolve<HttpClient>();
                AdviceResponse result = httpClient.GetJsonObject<AdviceResponse>(new Uri("http://fucking-great-advice.ru/api/random"));
                return result.Text.Replace("&nbsp;", " ").Replace("&#151;", "-");
            }
            catch (Exception)
            {
                return "Something bad just happened! No advices for you today.";
            }
        }
    }
}
