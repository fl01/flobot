using System;
using System.Collections.Generic;
using Flobot.ExternalServiceCore.Communication;

namespace Flobot.AccountsService.Storage
{
    public class MongoDbStorage : IUserStorage
    {
        public void AddUser(Caller user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser()
        {
            throw new NotImplementedException();
        }

        public Caller GetUser()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Caller> GetUsers()
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(Caller user)
        {
            throw new NotImplementedException();
        }
    }
}
