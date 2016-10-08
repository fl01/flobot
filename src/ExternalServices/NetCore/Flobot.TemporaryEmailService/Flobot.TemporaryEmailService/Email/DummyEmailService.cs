using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flobot.ExternalServiceCore.Communication;
using Flobot.TemporaryEmailService.Models;

namespace Flobot.TemporaryEmailService.Email
{
    public class DummyEmailService : IEmailService
    {
        public TemporaryEmail CreateEmail(Caller user)
        {
            return null;
        }

        public string GetEmailDisposeTimeForDisplay()
        {
            return null;
        }

        public IEnumerable<TemporaryEmail> GetHistoryRecords(Caller user)
        {
            return null;
        }
    }
}
