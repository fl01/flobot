using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Flobot.Common;
using Newtonsoft.Json;

namespace Flobot.Messages.Handlers.Advice
{
    public class AdviceProvider
    {
        public string GetAdvice()
        {
            try
            {
                using (SimpleJsonClient jsonClient = new SimpleJsonClient())
                {
                    AdviceResponse result = jsonClient.GetJsonObject<AdviceResponse>("http://fucking-great-advice.ru/api/random");

                    return result.Text.Replace("&nbsp;", " ").Replace("&#151;", "-");
                }
            }
            catch (Exception)
            {
                return "Something bad just happened! No advices for you today.";
            }
        }
    }
}
