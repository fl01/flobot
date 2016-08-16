using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages.Handlers.PictureStore
{
    public class PictureStoreStats
    {
        public int MaxPicturesLoad { get; set; }

        public int CurrentPicturesLoad { get; set; }

        public double StoreLoadPercentage
        {
            get
            {
                return (CurrentPicturesLoad / (double)MaxPicturesLoad) * 100;
            }
        }
    }
}