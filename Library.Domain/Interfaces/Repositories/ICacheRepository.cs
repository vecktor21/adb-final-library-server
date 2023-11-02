using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces.Repositories
{
    public interface ICacheRepository
    {
        IDistributedCache Cache { get; }
        bool IsConnected { get; set; }
        Task<bool> TryConnect();
        Task<TResult?> GetValue<TResult>(string key);
        Task<bool> SetValue(string key, string value, DistributedCacheEntryOptions options);
        Task<bool> SetValue(string key, string value);
    }
}

