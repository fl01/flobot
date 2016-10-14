using System.Collections.Generic;
using Flobot.ExternalServiceLocator.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Flobot.ExternalServiceLocator.Controllers
{
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        private IServicesStorage servicesStorage;

        public ServicesController(IServicesStorage servicesStorage)
        {
            this.servicesStorage = servicesStorage;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
