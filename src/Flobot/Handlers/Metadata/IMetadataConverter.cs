using Flobot.Messages;

namespace Flobot.Handlers.Metadata
{
    public interface IMetadataConverter
    {
        IMessageHandler Convert(IHandlerMetadata metadata);
    }
}