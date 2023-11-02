using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Common.Options
{
    public class ConnectionOptions 
    {
        public string MongoConnection { get; set; } = null!;
        public string Database { get; set; } = null!;
        public string UserCollectionName { get; set; } = null!;
        public string BookCollectionName { get; set; } = null!;
        public string CartCollectionName { get; set; } = null!;
        public string RedisConnection { get; set; } = null!;
    }
}
