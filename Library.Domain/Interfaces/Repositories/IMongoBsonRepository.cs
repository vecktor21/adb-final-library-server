using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces.Repositories
{
    public interface IMongoBsonRepository<T>
    {
        List<T> Find(BsonDocument bson);
        Task<List<T>> FindAsync(BsonDocument bson);
        /*List<T> Find(BsonDocument bson);
        Task<List<T>> FindAsync(BsonDocument bson);*/
    }
}
