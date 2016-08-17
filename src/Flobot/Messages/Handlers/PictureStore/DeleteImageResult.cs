using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages.Handlers.PictureStore
{
    public class DeleteImageResult : ActionResult
    {
        private DeleteImageResult()
        {
        }

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