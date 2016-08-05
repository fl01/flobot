﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flobot.Identity
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Role Role { get; set; }

        public Group Group { get; set; }
    }
}
