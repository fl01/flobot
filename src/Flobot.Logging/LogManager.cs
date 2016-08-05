using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using log4net.Config;

namespace Flobot.Logging
{
    public static class LogManager
    {
        static LogManager()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Path.Combine(Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path)), "Log4Net.config");
            FileInfo configFile = new FileInfo(path);

            if (!configFile.Exists)
            {
                return;
            }

            var repository = log4net.LogManager.GetRepository();
            XmlConfigurator.ConfigureAndWatch(repository, configFile);
        }

        public static ILog GetLogger(this object target)
        {
            if (target == null)
            {
                return GetLogger(typeof(object));
            }

            return GetLogger(target.GetType());
        }

        public static ILog GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }

        private static ILog GetLogger(Type target)
        {
            var logger = log4net.LogManager.GetLogger(target);
            return new Logger(logger);
        }

        private static string GetFormattedError(ICollection rawConfigErrors)
        {
            var errorMessages = rawConfigErrors.OfType<log4net.Util.LogLog>()
                .Select(error => string.Format("{0} [{1}] - {2}", error.TimeStamp, error.Prefix, error.Message));

            return string.Join(Environment.NewLine, errorMessages);
        }
    }
}
