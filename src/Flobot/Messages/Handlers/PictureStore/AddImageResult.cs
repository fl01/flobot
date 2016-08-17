using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;

namespace Flobot.Messages.Handlers.PictureStore
{
    public class AddImageResult : ActionResult
    {
        public ImagePath Image { get; private set; }

        public override bool Success
        {
            get
            {
                return base.Success && Image != null && Image.Exists;
            }
            protected set
            {
                base.Success = value;
            }
        }

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