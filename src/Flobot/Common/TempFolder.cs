using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Flobot.Common.Container;
using Flobot.Logging;

namespace Flobot.Common
{
    public class TempFolder : IDisposable
    {
        private ILog logger;
        private string folderPath;

        public string FolderPath
        {
            get
            {
                return folderPath;
            }
            private set
            {
                if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath) && !folderPath.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                {
                    DeleteDirectory(folderPath);
                }

                folderPath = value;
                CreateDirectory(folderPath);
            }
        }

        private string TempFolderRoot
        {
            get
            {
                return HttpContext.Current.Request.MapPath("~/TempRoot");
            }
        }

        public TempFolder()
        {
            FolderPath = Path.Combine(TempFolderRoot, Guid.NewGuid().ToString("D"));
            logger = IoC.Container.Resolve<ILoggingService>().GetLogger(this);
        }

        public void Dispose()
        {
            DeleteDirectory(FolderPath);
        }

        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (UnauthorizedAccessException ex)
                {
                    logger.Error(ex);
                }
            }
        }

        private void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, true);
                }
                catch (UnauthorizedAccessException ex)
                {
                    logger.Error(ex);
                }
            }
        }
    }
}