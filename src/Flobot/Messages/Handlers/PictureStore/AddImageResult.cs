using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;

namespace Flobot.Messages.Handlers.PictureStore
{
    public class AddImageResult
    {
        private bool success;

        public ImagePath Image { get; private set; }

        public bool Success
        {
            get
            {
                return success && Image != null && Image.Exists;
            }
            private set
            {
                success = value;
            }
        }

        public string Error { get; private set; }

        private AddImageResult()
        {
        }

        public static AddImageResult CreateFailResult(string errorMesage)
        {
            return new AddImageResult()
            {
                Success = false,
                Error = errorMesage
            };
        }

        public static AddImageResult CreateSuccessResult(ImagePath imagePath)
        {
            return new AddImageResult()
            {
                Success = true,
                Image = imagePath
            };
        }
    }
}