﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Identity
{
    [Flags]
    public enum Group
    {
        Default = 1,
        PsychoRaid = 2,
        Administrators = 4
    }
}
