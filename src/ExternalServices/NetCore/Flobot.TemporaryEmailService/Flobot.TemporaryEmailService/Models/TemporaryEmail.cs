using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flobot.TemporaryEmailService.Models
{
    public class TemporaryEmail
    {
        public string AccessLink { get; set; }   

        public string IssuerID { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime IssueDate { get; set; }

        public bool IsExpired
        {
            get { return DateTime.Now > ExpiryDate; }
        }
    }
}
