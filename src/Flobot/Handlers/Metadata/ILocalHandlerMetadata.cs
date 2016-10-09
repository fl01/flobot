using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Handlers.Metadata
{
    public interface ILocalHandlerMetadata : IHandlerMetadata
    {
        Type MessageHandlerType { get; }
    }
}