using System;
using Microsoft.Practices.Unity;

namespace Flobot.InversionOfControl
{
    public static class Extensions
    {
        public static LifetimeManager ToUnityLifetimeManager(this Lifetime lifetime)
        {
            switch (lifetime)
            {
                case Lifetime.PerResolve:
                    return new PerResolveLifetimeManager();
                case Lifetime.PerThread:
                    return new PerThreadLifetimeManager();
                case Lifetime.Singleton:
                    return new ContainerControlledLifetimeManager();
                default:
                    throw new NotSupportedException(string.Format("Value {0} is not supported", lifetime.ToString()));
            }
        }
    }
}
