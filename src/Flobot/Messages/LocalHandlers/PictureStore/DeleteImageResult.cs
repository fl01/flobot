namespace Flobot.Messages.LocalHandlers.PictureStore
{
    public class DeleteImageResult : ActionResult
    {
        public static DeleteImageResult CreateFailResult(string errorMesage)
        {
            return new DeleteImageResult()
            {
                Success = false,
                Error = errorMesage
            };
        }

        public static DeleteImageResult CreateSuccessResult()
        {
            return new DeleteImageResult()
            {
                Success = true
            };
        }
    }
}