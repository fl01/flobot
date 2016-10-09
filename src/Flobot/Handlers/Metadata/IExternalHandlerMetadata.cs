using Flobot.Common.DTO;

namespace Flobot.Handlers.Metadata
{
    public interface IExternalHandlerMetadata : IHandlerMetadata
    {
        ExternalConnectionDataDTO ConnectionData { get; }
    }
}