using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Flobot.Identity;
using Flobot.Messages.Handlers.Currency;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("currency", "curr")]
    public class CurrencyHandler : MessageHandlerBase
    {
        private const string CurrencyNbuBuySellLineFormat = "NBU : {0}" + SkypeNewLine;
        private const string CurrencyInterbankBuySellLineFormat = "Interbank : sell {0} ({2}), buy {1} ({3})" + SkypeNewLine;

        private ICurrencyProxy proxy;

        private ICurrencyProxy Proxy
        {
            get
            {
                if (proxy == null)
                {
                    proxy = new OpenratesProxy();
                }

                return proxy;
            }
        }

        public CurrencyHandler(User caller, Message message)
            : base(caller, message)
        {
        }

        protected override string GetReplyMessage(Activity activity)
        {
            CurrencyContainer currencyInfo = Proxy.GetCurrencyInfo();
            CurrencyContainer previousDayInfo = Proxy.GetCurrencyInfo(DateTime.Now.AddDays(-1));

            return GetFormattedCurrency(currencyInfo, previousDayInfo);
        }

        private string GetFormattedCurrency(CurrencyContainer currencyInfo, CurrencyContainer previosDayInfo)
        {
            string usdInterbankSellDiff = GetCurrencyDiff(previosDayInfo.USD.Interbank.Sell, currencyInfo.USD.Interbank.Sell);
            string usdInterbankBuyDiff = GetCurrencyDiff(previosDayInfo.USD.Interbank.Buy, currencyInfo.USD.Interbank.Buy);
            string eurInterbankSellDiff = GetCurrencyDiff(previosDayInfo.EUR.Interbank.Sell, currencyInfo.EUR.Interbank.Sell);
            string eurInterbankBuyDiff = GetCurrencyDiff(previosDayInfo.EUR.Interbank.Buy, currencyInfo.EUR.Interbank.Buy);
            string rubInterbankSellDiff = GetCurrencyDiff(previosDayInfo.RUB.Interbank.Sell, currencyInfo.RUB.Interbank.Sell);
            string rubInterbankBuyDiff = GetCurrencyDiff(previosDayInfo.RUB.Interbank.Buy, currencyInfo.RUB.Interbank.Buy);

            StringBuilder sb = new StringBuilder();

            sb.Append("--USD--" + SkypeNewLine);
            sb.AppendFormat(CurrencyNbuBuySellLineFormat, currencyInfo.USD.Nbu.Buy);
            sb.AppendFormat(CurrencyInterbankBuySellLineFormat, currencyInfo.USD.Interbank.Sell, currencyInfo.USD.Interbank.Buy, usdInterbankSellDiff, usdInterbankBuyDiff);

            sb.Append("--EUR--" + SkypeNewLine);
            sb.AppendFormat(CurrencyNbuBuySellLineFormat, currencyInfo.EUR.Nbu.Buy);
            sb.AppendFormat(CurrencyInterbankBuySellLineFormat, currencyInfo.EUR.Interbank.Sell, currencyInfo.EUR.Interbank.Buy, eurInterbankSellDiff, eurInterbankBuyDiff);

            sb.Append("--RUB--" + SkypeNewLine);
            sb.AppendFormat(CurrencyNbuBuySellLineFormat, currencyInfo.RUB.Nbu.Buy);
            sb.AppendFormat(CurrencyInterbankBuySellLineFormat, currencyInfo.RUB.Interbank.Sell, currencyInfo.RUB.Interbank.Buy, rubInterbankSellDiff, rubInterbankBuyDiff);

            return sb.ToString();
        }

        private string GetCurrencyDiff(double previous, double current)
        {
            double diffPercent = Math.Round((previous - current) / previous, 4);

            string formattedDiffPercent = $"{diffPercent}%";

            if (diffPercent > 0)
            {
                return "+" + formattedDiffPercent;
            }

            return formattedDiffPercent;
        }
    }
}
