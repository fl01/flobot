using System.Collections.Generic;
using Flobot.ExternalServiceCore.Communication;
using Flobot.TemporaryEmailService.Models;

namespace Flobot.TemporaryEmailService.Email
{
    public interface IEmailService
    {
        TemporaryEmail CreateEmail(Caller user);

        IEnumerable<TemporaryEmail> GetHistoryRecords(Caller user);

        string GetEmailDisposeTimeForDisplay();
    }
}
