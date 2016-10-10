using System.Collections.Generic;
using Flobot.ExternalServiceCore.Communication;

namespace Flobot.AccountsService.Storage
{
    public interface IUserStorage
    {
        IEnumerable<Caller> GetUsers();

        Caller GetUser();

        void AddUser(Caller user);

        void DeleteUser();

        void UpdateUser(Caller user);
    }
}
