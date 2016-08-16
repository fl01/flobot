using System;
using System.Collections.Generic;
using Flobot.Common;
using Flobot.Identity;
using Flobot.Messages.Handlers.Currency;
using Microsoft.Bot.Connector;

namespace Flobot.Messages.Handlers
{
    [Permissions(Role.User)]
    [Message("Be in touch with exchange rates", Section.Default, "currency", "curr")]
    public class CurrencyHandler : MessageHandlerBase
    {
        private const string CurrencyNbuBuySellLineFormat = "NBU : {0}";
        private const string CurrencyInterbankBuySellLineFormat = "Interbank : sell {0} ({2}), buy {1} ({3})";

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

        public CurrencyHandler(ActivityBundle activityBundle)
            : base(activityBundle)
        {
        }

        protected override IEnumerable<Activity> CreateReplies()
        {
            string replyMessage = GetReplyMessage();
            return new[] { ActivityBundle.Activity.CreateReply(replyMessage) };
        }

        private string GetReplyMessage()
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

            StringBuilderEx sb = new StringBuilderEx(StringBuilderExMode.Skype);

            sb.AppendLine("--USD--");
            sb.AppendFormatLine(CurrencyNbuBuySellLineFormat, currencyInfo.USD.Nbu.Buy);
            sb.AppendFormatLine(CurrencyInterbankBuySellLineFormat, currencyInfo.USD.Interbank.Sell, currencyInfo.USD.Interbank.Buy, usdInterbankSellDiff, usdInterbankBuyDiff);

            sb.AppendLine("--EUR--");
            sb.AppendFormatLine(CurrencyNbuBuySellLineFormat, currencyInfo.EUR.Nbu.Buy);
            sb.AppendFormatLine(CurrencyInterbankBuySellLineFormat, currencyInfo.EUR.Interbank.Sell, currencyInfo.EUR.Interbank.Buy, eurInterbankSellDiff, eurInterbankBuyDiff);

            sb.AppendLine("--RUB--");
            sb.AppendFormatLine(CurrencyNbuBuySellLineFormat, currencyInfo.RUB.Nbu.Buy);
            sb.AppendFormatLine(CurrencyInterbankBuySellLineFormat, currencyInfo.RUB.Interbank.Sell, currencyInfo.RUB.Interbank.Buy, rubInterbankSellDiff, rubInterbankBuyDiff);

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
