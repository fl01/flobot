using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flobot.ExternalServiceCore.Communication;
using Flobot.TemporaryEmailService.Email;
using Flobot.TemporaryEmailService.Models;
using Flobot.TemporaryEmailService.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Flobot.TemporaryEmailService.Controllers
{
    [Route("api/[controller]")]
    public class MainController : Controller
    {
        private IEmailService emailService;

        public MainController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello from .net core";
        }

        [HttpPost]
        public ExternalReply[] Post([FromBody]Request request)
        {
            // TODO : check command
            return CreateNewTemporaryEmail(request);
        }

        private ExternalReply[] CreateNewTemporaryEmail(Request request)
        {
            TemporaryEmail email = emailService.CreateEmail(request.Caller);
            string replyText;
            if (email == null)
            {
                replyText = "Failed to create email on remote service. Please try again later";
            }
            else
            {
                replyText = $"{request.Caller.Name}, new email has been generated at {email.IssueDate}. Email: '{email.Email}', Dispose Time: {emailService.GetEmailDisposeTimeForDisplay()}, AccessLink: {email.AccessLink}";
            }

            return ExternalReplyFactory.CreateSingleTextReplyCollection(replyText);
        }
    }
}
