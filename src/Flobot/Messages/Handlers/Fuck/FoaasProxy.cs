using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Flobot.Common;
using Newtonsoft.Json;

namespace Flobot.Messages.Handlers.Fuck
{
    public class FoaasProxy
    {
        private List<string> fromReplyLinks;
        private List<string> fromToNameReplyLinks;

        public FoaasProxy()
        {
            fromReplyLinks = new List<string>()
            {
                "http://foaas.com/this/{0}",
                "http://foaas.com/that/{0}",
                "http://foaas.com/everything/{0}",
                "http://foaas.com/everyone/{0}",
                "http://foaas.com/pink/{0}",
                "http://foaas.com/life/{0}",
                "http://foaas.com/thing/{0}",
                "http://foaas.com/thanks/{0}",
                "http://foaas.com/flying/{0}",
                "http://foaas.com/fascinating/{0}",
                "http://foaas.com/cool/{0}",
                "http://foaas.com/what/{0}",
                "http://foaas.com/because/{0}",
                "http://foaas.com/bye/{0}",
                "http://foaas.com/diabetes/{0}",
                "http://foaas.com/awesome/{0}",
                "http://foaas.com/tucker/{0}",
                "http://foaas.com/bucket/{0}",
                "http://foaas.com/family/{0}",
                "http://foaas.com/zayn/{0}",
                "http://foaas.com/mornin/{0}",
                "http://foaas.com/retard/{0}",
                "http://foaas.com/me/{0}",
                "http://foaas.com/single/{0}",
                "http://foaas.com/no/{0}",
                "http://foaas.com/give/{0}",
                "http://foaas.com/zero/{0}",
                "http://foaas.com/sake/{0}"
            };

            fromToNameReplyLinks = new List<string>()
            {
                "http://foaas.com/off/{0}/{1}",
                "http://foaas.com/you/{0}/{1}",
                "http://foaas.com/donut/{0}/{1}",
                "http://foaas.com/shakespeare/{0}/{1}",
                "http://foaas.com/linus/{0}/{1}",
                "http://foaas.com/king/{0}/{1}",
                "http://foaas.com/chainsaw/{0}/{1}",
                "http://foaas.com/outside/{0}/{1}",
                "http://foaas.com/madison/{0}/{1}",
                "http://foaas.com/nugget/{0}/{1}",
                "http://foaas.com/yoda/{0}/{1}",
                "http://foaas.com/bus/{0}/{1}",
                "http://foaas.com/xmas/{0}/{1}",
                "http://foaas.com/bday/{0}/{1}",
                "http://foaas.com/shutup/{0}/{1}",
                "http://foaas.com/gfy/{0}/{1}",
                "http://foaas.com/back/{0}/{1}",
                "http://foaas.com/think/{0}/{1}",
                "http://foaas.com/keep/{0}/{1}",
                "http://foaas.com/look/{0}/{1}"
            };
        }

        public string GetRandomFromReply(string from)
        {
            var link = fromReplyLinks[new Random().Next(fromReplyLinks.Count)];

            string formattedLink = string.Format(link, from);
            Uri url = new Uri(formattedLink);
            FoaasResponse reply = GetFoaasReply(url);

            return reply.ToString();
        }

        public string GetRandomFromToNameReply(string name, string from)
        {
            var link = fromToNameReplyLinks[new Random().Next(fromToNameReplyLinks.Count)];

            string formattedLink = string.Format(link, name, from);
            Uri url = new Uri(formattedLink);
            FoaasResponse reply = GetFoaasReply(url);

            return reply.Message;
        }

        private FoaasResponse GetFoaasReply(Uri url)
        {
            try
            {
                using (SimpleJsonClient wc = new SimpleJsonClient())
                {
                    FoaasResponse response = wc.GetJsonObject<FoaasResponse>(url);
                    return response;
                }
            }
            catch (Exception)
            {
                // TODO : log
                return new FoaasResponse() { Message = "Something went wrong! Try again later." };
            }
        }
    }
}
