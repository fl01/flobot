using System;

namespace Flobot.Logging
{
    public class LoggingService : ILoggingService
    {
        public ILog GetLogger(object target)
        {
            return LogManager.GetLogger(target);
        }

        public ILog GetLogger<T>()
        {
            return LogManager.GetLogger<T>();
        }
    }
}
