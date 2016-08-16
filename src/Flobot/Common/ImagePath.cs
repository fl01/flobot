using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Flobot.Common
{
    public class ImagePath
    {
        private const string RootImageFolder = @"Images";
        private const string WebPathSlash = @"/";
        private const string PhysicalPathSlash = @"\";

        private string imageName;
        private string imageSubfolder;

        public string WebPath
        {
            get
            {
                return string.Join(WebPathSlash, HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), RootImageFolder, imageSubfolder, imageName);
            }
        }

        public string PhysicalPath
        {
            get
            {
                return HttpContext.Current.Request.MapPath(string.Join(PhysicalPathSlash, "~", RootImageFolder, imageSubfolder, imageName));
            }
        }

        public bool Exists
        {
            get
            {
                return File.Exists(PhysicalPath);
            }
        }

        public ImagePath(string imageName, string imageSubfolder)
        {
            this.imageName = imageName;
            this.imageSubfolder = imageSubfolder;
        }
    }
}
