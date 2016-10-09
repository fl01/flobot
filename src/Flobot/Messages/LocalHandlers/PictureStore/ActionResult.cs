namespace Flobot.Messages.LocalHandlers.PictureStore
{
    public abstract class ActionResult
    {
        public virtual bool Success { get; protected set; }

        public string Error { get; set; }
    }
}