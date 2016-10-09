using System;
using System.Collections.Generic;
using System.Linq;
using Flobot.Handlers.Metadata;

namespace Flobot.Handlers.DataSource
{
    public class ExternalServicesDataSource : IMetadataDataSource
    {
        public IEnumerable<IHandlerMetadata> GetAll()
        {
            return Enumerable.Empty<IHandlerMetadata>();
        }
    }
}