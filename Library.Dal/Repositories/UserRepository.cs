using Library.Common.Options;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Dal.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;

namespace Library.Dal.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Database db;
        private readonly ILogger logger;
        private readonly ConnectionOptions options;
        private string collectionName;

        public UserRepository(Database db, IOptions<ConnectionOptions> options, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
            this.options = options.Value;
        }

        public async Task<IUser?> FindUser(Guid id)
        {
            return await db.GetCollection<UserEntity>(options.UserCollectionName).Find(x=>x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<IUser>> GetUsers()
        {
            IEnumerable<IUser> users = db.GetCollection<UserEntity>(options.UserCollectionName).Find("{}").ToList();
            return users.ToList();
        }

        public async Task<bool> UpdateUser(IUser user)
        {
            try
            {
                var updateDefinition = Builders<IUser>.Update
                .Set(x => x.Age, user.Age)
                .Set(x => x.BooksViewHistory, user.BooksViewHistory)
                .Set(x => x.Name, user.Name)
                .Set(x => x.Surname, user.Surname)
                .Set(x => x.PhoneNumber, user.PhoneNumber)
                .Set(x => x.Email, user.Email)
                .Set(x => x.Description, user.Description);
                var filter = Builders<IUser>.Filter
                    .Eq(x => x.Id, user.Id);

                var updateResult = await db.GetCollection<IUser>(options.UserCollectionName).UpdateOneAsync(filter, updateDefinition);
                
                return updateResult.IsAcknowledged;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
        }

        async Task<bool> IUserRepository.DeleteUser(Guid id)
        {
            try
            {
                var filter = Builders<IUser>.Filter
                    .Eq(x => x.Id, id);

                var updateResult = await db.GetCollection<IUser>(options.UserCollectionName).DeleteOneAsync(filter);

                return updateResult.IsAcknowledged;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
        }
    }
}
