namespace Flobot.Messages.LocalHandlers.PictureStore
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