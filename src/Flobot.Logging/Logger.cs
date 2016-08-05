using System;

namespace Flobot.Logging
{
    internal class Logger : ILog
    {
        private log4net.ILog currentLogger;

        public Logger(log4net.ILog logger)
        {
            currentLogger = logger;
        }

        #region Properties

        public bool IsDebugEnabled
        {
            get { return currentLogger.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return currentLogger.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return currentLogger.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return currentLogger.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return currentLogger.IsFatalEnabled; }
        }

        #endregion // Properties

        #region Methods

        public void Debug(object message)
        {
            currentLogger.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            currentLogger.Debug(message, exception);
        }

        public void Info(object message)
        {
            currentLogger.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            currentLogger.Info(message, exception);
        }

        public void Warn(object message)
        {
            currentLogger.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            currentLogger.Warn(message, exception);
        }

        public void Error(object message)
        {
            currentLogger.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            currentLogger.Error(message, exception);
        }

        public void Fatal(object message)
        {
            currentLogger.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            currentLogger.Fatal(message, exception);
        }

        #endregion // Methods
    }
}
