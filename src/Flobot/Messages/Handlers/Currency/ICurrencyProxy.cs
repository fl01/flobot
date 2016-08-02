using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages.Handlers.Currency
{
    public interface ICurrencyProxy
    {
        CurrencyContainer GetCurrencyInfo();

        CurrencyContainer GetCurrencyInfo(DateTime date);
    }
}
