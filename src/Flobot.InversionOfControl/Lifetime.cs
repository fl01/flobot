using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flobot.InversionOfControl
{
    public enum Lifetime
    {
        PerResolve,
        PerThread,
        Singleton
    }
}
