using System.Collections.Generic;
using Flobot.Handlers.DataSource;
using Flobot.Handlers.Metadata;

namespace Flobot.Handlers
{
    public interface IHandlerMetadataService
    {
        IReadOnlyCollection<IHandlerMetadata> GetHandlersMetadata();

        IHandlerMetadataService AddDataSource(IMetadataDataSource dataSource);

        void EnsureInitialized();
    }
}