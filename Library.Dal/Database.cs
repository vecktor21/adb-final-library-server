using Library.Dal.Models;
using Library.Common.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal
{
    public class Database 
    {
        private readonly string databaseName;
        private readonly ConnectionOptions options;
        private readonly ILogger logger;
        private MongoClient db;

        public Database(IOptions<ConnectionOptions> options, ILogger logger)
        {
            logger.Debug($"Setting connection with: Database: {options.Value.Database};");
            logger.Debug($"Connection string: {options.Value.MongoConnection}");
            this.databaseName = options.Value.Database;
            this.options = options.Value;
            db = new MongoClient(options.Value.MongoConnection);
            this.logger = logger;
            logger.Debug(messageTemplate: $"Successfully established connection");
        }

        public IMongoDatabase GetConnection()
        {
            return db.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName) 
        {
            return GetConnection().GetCollection<T>(collectionName);
        }
    }
}
