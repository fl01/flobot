using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages.LocalHandlers.Currency
{
    public class CurrencyData
    {
        public CurrencyValue Interbank { get; set; }

        public CurrencyValue Nbu { get; set; }
    }
}
