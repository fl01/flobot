using System;
using System.Collections.Generic;
using System.Linq;
using Flobot.AccountsService.Settings;
using MongoDB.Driver;
using Flobot.ExternalServiceCore.Communication;
using MongoDB.Bson;
using Flobot.ExternalServiceCore.Identity;

namespace Flobot.AccountsService.Storage
{
    public class MongoDbStorage : IUserStorage
    {
        private const string UserCollectionName = "{473D4464-E9EB-4688-AD29-C62DA0CB3487}";

        private IMongoClient client;
        private IMongoDatabase database;
        private ISettingsService settingsService;

        private IMongoCollection<Caller> UserCollection
        {
            get { return database.GetCollection<Caller>(UserCollectionName); }
        }

        public MongoDbStorage(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
            client = new MongoClient(settingsService.GetConnectionString());
            database = client.GetDatabase(settingsService.GetDbName());
        }

        public StorageActionResult AddUser(Caller user)
        {
            try
            {
                if (user == null)
                {
                    return new StorageActionResult(ResultType.Fail, new[] { $"Null user cannot be added" });
                }

                if (UserCollection.Find(u => u.Id == user.Id).Any())
                {
                    return new StorageActionResult(ResultType.Fail, new[] { $"User '{user.Id}' already exists" });
                }

                UserCollection.InsertOne(user);
                return new StorageActionResult(ResultType.Success);
            }
            catch (Exception ex)
            {
                // TODO : logs
                return new StorageActionResult(ResultType.Fail, new[] { ex.Message });
            }
        }

        public StorageActionResult DeleteUser(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return new StorageActionResult(ResultType.Fail, new[] { $"Invalid user ID" });
                }

                var deleted = UserCollection.DeleteOne(u => u.Id == id).DeletedCount;

                if (deleted > 0)
                {
                    return new StorageActionResult(ResultType.Success);
                }
                else
                {
                    return new StorageActionResult(ResultType.Fail, new[] { $"The user '{id}' has not been deleted" });
                }
            }
            catch (Exception ex)
            {
                // TODO : logs
                return new StorageActionResult(ResultType.Fail, new[] { ex.Message });
            }
        }

        public Caller GetUser(string id)
        {
            return UserCollection
                .Find(u => u.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Caller> GetUsers()
        {
            return UserCollection
                .Find(f => true)
                .ToList();
        }

        public StorageActionResult UpdateUser(Caller user)
        {
            try
            {
                if (user == null)
                {
                    return new StorageActionResult(ResultType.Fail, new[] { $"Null user cannot be deleted" });
                }

                if (string.IsNullOrEmpty(user.Id))
                {
                    return new StorageActionResult(ResultType.Fail, new[] { $"Invalid user ID" });
                }

                var update = new UpdateDefinitionBuilder<Caller>()
                    .Set(u => u.Role, user.Role)
                    .Set(u => u.Group, user.Group);

                var modifiedCount = UserCollection.UpdateOne<Caller>(x => x.Id == user.Id, update).ModifiedCount;

                if (modifiedCount > 0)
                {
                    return new StorageActionResult(ResultType.Success);
                }
                else
                {
                    return new StorageActionResult(ResultType.Fail, new[] { $"The user '{user.Id}' has not been updated" });
                }
            }
            catch (Exception ex)
            {
                // TODO : logs
                return new StorageActionResult(ResultType.Fail, new[] { ex.Message });
            }
        }
    }
}
