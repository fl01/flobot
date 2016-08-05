using System;

namespace Flobot.Logging
{
    public interface ILoggingService
    {
        ILog GetLogger(object target);

        ILog GetLogger<T>();
    }
}
