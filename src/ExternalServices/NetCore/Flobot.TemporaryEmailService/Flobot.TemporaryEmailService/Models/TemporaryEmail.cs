using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flobot.TemporaryEmailService.Models
{
    public class TemporaryEmail
    {
        public string AccessLink { get; set; }

        public string UserID { get; set; }

        public string Email { get; set; }

        public DateTime IssueDate { get; set; }
    }
}
