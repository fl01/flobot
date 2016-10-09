using System;
using Flobot.Messages;

namespace Flobot.Handlers.Metadata
{
    public class MetadataConverter : IMetadataConverter
    {
        public IMessageHandler Convert(IHandlerMetadata metadata)
        {
            switch (metadata.HandlerType)
            {
                case HandlerType.Local:
                    return ConvertLocal(metadata as ILocalHandlerMetadata);
                case HandlerType.External:
                    return ConvertExternal(metadata as IExternalHandlerMetadata);
                default:
                    throw new NotSupportedException();
            }
        }

        private IMessageHandler ConvertExternal(IExternalHandlerMetadata externalMetadata)
        {
            if (externalMetadata == null)
            {
                return null;
            }

            return null;
        }

        private IMessageHandler ConvertLocal(ILocalHandlerMetadata localMetadata)
        {
            if (localMetadata == null || localMetadata.MessageHandlerType == null)
            {
                return null;
            }

            return Activator.CreateInstance(localMetadata.MessageHandlerType) as IMessageHandler;
        }
    }
}