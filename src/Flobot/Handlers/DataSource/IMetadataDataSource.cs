using System.Collections.Generic;
using Flobot.Handlers.Metadata;

namespace Flobot.Handlers.DataSource
{
    public interface IMetadataDataSource
    {
        IEnumerable<IHandlerMetadata> GetAll();
    }
}