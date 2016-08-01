using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace Flobot.Messages.Handlers.Advice
{
    public class AdviceProvider
    {
        public string GetAdvice()
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var rawHtml = wc.DownloadString("http://fucking-great-advice.ru/api/random");
                    AdviceResponse result = JsonConvert.DeserializeObject<AdviceResponse>(rawHtml);

                    if (result != null && !string.IsNullOrEmpty(result.Text))
                    {
                        return result.Text.Replace("&nbsp;", " ");
                    }

                }
            }
            catch (Exception)
            {
            }

            return "Something bad just happened! No advices for you today.";
        }
    }
}
