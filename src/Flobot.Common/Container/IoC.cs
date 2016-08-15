using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flobot.InversionOfControl;
using Flobot.Logging;

namespace Flobot.Common.Container
{
    public static class IoC
    {
        public static DependencyContainer Container { get; private set; }

        static IoC()
        {
            Container = DependencyContainer.New()
                .Register<ILoggingService, LoggingService>(Lifetime.Singleton);
        }
    }
}
