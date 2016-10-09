using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;
using Flobot.Common.Container;
using Flobot.Common.Net;

namespace Flobot.Messages.LocalHandlers.Currency
{
    public class OpenratesProxy : ICurrencyProxy
    {
        private const string Url = "http://openrates.in.ua/rates?date=";
        private const string RequestDateFormat = "yyyy-MM-dd";

        public CurrencyContainer GetCurrencyInfo()
        {
            return GetCurrencyInfo(DateTime.Now);
        }

        public CurrencyContainer GetCurrencyInfo(DateTime date)
        {
            var httpClient = IoC.Container.Resolve<HttpClient>();
            Uri url = new Uri(Url + date.ToString(RequestDateFormat));
            return httpClient.GetJsonObject<CurrencyContainer>(url);
        }
    }
}
