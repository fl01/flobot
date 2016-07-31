using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Flobot.Identity;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("advice", "adv")]
    public class AdviceHandler : MessageHandlerBase
    {
        private EasyAdviceProvider adviceProvider;

        public AdviceHandler(Message message)
            : base(message)
        {
            adviceProvider = new EasyAdviceProvider();
        }

        protected override string GetReplyMessage(Activity activity)
        {
            return adviceProvider.GetAdvice();
        }

        private class EasyAdviceProvider
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

            private class AdviceResponse
            {
                public string Text { get; set; }
            }
        }
    }
}
