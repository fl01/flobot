using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;

namespace Flobot.Messages.Handlers.Currency
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
            using (SimpleJsonClient jsonClient = new SimpleJsonClient())
            {
                string url = Url + date.ToString(RequestDateFormat);
                CurrencyContainer currency = jsonClient.GetJsonObject<CurrencyContainer>(url);

                return currency;
            }
        }
    }
}
