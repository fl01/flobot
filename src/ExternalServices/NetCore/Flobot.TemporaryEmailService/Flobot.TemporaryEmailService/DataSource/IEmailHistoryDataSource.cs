using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flobot.ExternalServiceCore.Communication;
using Flobot.TemporaryEmailService.Models;

namespace Flobot.TemporaryEmailService.DataSource
{
    public interface IEmailHistoryDataSource
    {
        IEnumerable<TemporaryEmail> GetLastRecords(Caller user);

        bool Save(TemporaryEmail email);
    }
}
