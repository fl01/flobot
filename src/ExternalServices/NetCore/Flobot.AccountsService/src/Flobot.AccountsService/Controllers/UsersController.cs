using System.Collections.Generic;
using System.Net;
using Flobot.AccountsService.Storage;
using Flobot.ExternalServiceCore.Communication;
using Microsoft.AspNetCore.Mvc;

namespace Flobot.AccountsService.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserStorage userStorage;

        public UsersController(IUserStorage userStorage)
        {
            this.userStorage = userStorage;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Caller> Get()
        {
            return userStorage
                .GetUsers();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Caller Get(string id)
        {
            return userStorage.GetUser(id);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Caller user)
        {
            StorageActionResult result = userStorage.AddUser(user);

            return GetResponse(result, HttpStatusCode.Created);
        }

        // PUT api/values/5
        [HttpPut]
        public IActionResult Put([FromBody]Caller user)
        {
            StorageActionResult result = userStorage.UpdateUser(user);

            return GetResponse(result, HttpStatusCode.OK);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            StorageActionResult result = userStorage.DeleteUser(id);

            return GetResponse(result, HttpStatusCode.OK);
        }

        private IActionResult GetResponse(StorageActionResult storageResult, HttpStatusCode success)
        {
            if (storageResult.Result == ResultType.Fail)
            {
                return new BadRequestObjectResult(storageResult.Errors);
            }

            return new StatusCodeResult((int)success);
        }
    }
}
