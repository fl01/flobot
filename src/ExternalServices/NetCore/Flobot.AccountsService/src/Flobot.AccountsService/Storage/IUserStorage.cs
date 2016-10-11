using System.Collections.Generic;
using Flobot.ExternalServiceCore.Communication;

namespace Flobot.AccountsService.Storage
{
    public interface IUserStorage
    {
        IEnumerable<Caller> GetUsers();

        Caller GetUser(string id);

        StorageActionResult AddUser(Caller user);

        StorageActionResult DeleteUser(string id);

        StorageActionResult UpdateUser(Caller user);
    }
}
