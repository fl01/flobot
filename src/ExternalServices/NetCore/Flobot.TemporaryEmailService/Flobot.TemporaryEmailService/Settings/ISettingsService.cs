using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flobot.TemporaryEmailService.Settings
{
    public interface ISettingsService
    {
        string GetConnectionString();
    }
}
