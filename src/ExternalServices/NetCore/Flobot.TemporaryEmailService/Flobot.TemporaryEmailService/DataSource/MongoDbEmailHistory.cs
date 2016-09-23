using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flobot.ExternalServiceCore.Communication;
using Flobot.TemporaryEmailService.Models;
using Flobot.TemporaryEmailService.Settings;

namespace Flobot.TemporaryEmailService.DataSource
{
    public class MongoDbEmailHistory : IEmailHistoryDataSource
    {
        private ISettingsService settings;

        public MongoDbEmailHistory(ISettingsService settings)
        {
            this.settings = settings;
        }

        public IEnumerable<TemporaryEmail> GetLastRecords(Caller user)
        {
            return Enumerable.Empty<TemporaryEmail>();
        }

        public bool Save(TemporaryEmail email)
        {
            return true;
        }
    }
}
