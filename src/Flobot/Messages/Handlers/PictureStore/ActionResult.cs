using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Messages.Handlers.PictureStore
{
    public abstract class ActionResult
    {
        public virtual bool Success { get; protected set; }

        public string Error { get; set; }

        protected ActionResult()
        {
        }
    }
}