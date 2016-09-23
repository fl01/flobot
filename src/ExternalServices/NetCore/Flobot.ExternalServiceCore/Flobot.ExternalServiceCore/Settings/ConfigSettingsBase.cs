using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Flobot.ExternalServiceCore.Settings
{
    public abstract class ConfigSettingsBase
    {
        private IConfigurationRoot root;

        public string GetKeyValue(string key)
        {
            return root[key];
        }

        public void SetKeyValue(string key, string value)
        {
            root[key] = value;
        }

        public ConfigSettingsBase(IConfigurationRoot root)
        {
            this.root = root;
        }
    }
}
