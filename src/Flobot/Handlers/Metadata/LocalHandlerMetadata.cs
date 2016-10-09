using System;

namespace Flobot.Handlers.Metadata
{
    public class LocalHandlerMetadata : HandlerMetadataBase, ILocalHandlerMetadata
    {
        public override HandlerType HandlerType
        {
            get { return HandlerType.Local; }
        }

        public Type MessageHandlerType { get; set; }
    }
}