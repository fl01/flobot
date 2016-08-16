using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flobot.Common;

namespace Flobot.Messages.Handlers.PictureStore
{
    public interface IPictureStore
    {
        IList<ImagePath> GetAllPictures();

        PictureStoreStats GetStats();

        AddImageResult Add(string imageName, Uri imageUrl);
    }
}