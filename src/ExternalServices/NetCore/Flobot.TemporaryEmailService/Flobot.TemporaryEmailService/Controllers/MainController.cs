using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flobot.ExternalServiceCore.Communication;
using Microsoft.AspNetCore.Mvc;

namespace Flobot.TemporaryEmailService.Controllers
{
    [Route("api/[controller]")]
    public class MainController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            // TODO : return help
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public ExternalReply[] Post([FromBody]Request request)
        {
            var dummyReply = new ExternalReply()
            {
                Kind = ReplyType.Text,
                Text = "ALL HAIL .NET CORE"
            };

            return new[] { dummyReply };
        }
    }
}
