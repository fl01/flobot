using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Identity
{
    public interface IPermissionsService
    {
        bool HasPermissions(Role callerRole, Group callerGroup, Role targetRole, Group targetGroup);
    }
}