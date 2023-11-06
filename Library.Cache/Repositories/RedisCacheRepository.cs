using Library.Common.Options;
using Library.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Cache.Repositories
{
    public class RedisCacheRepository : ICacheRepository
    {
        private readonly ILogger logger;
        private readonly RedisOptions options;
        public bool IsConnected { get; set; }
        public IDistributedCache Cache { get; }
        private DistributedCacheEntryOptions defaultOptions;
        private DateTimeOffset ConnectionErrorTime;

        public RedisCacheRepository(IDistributedCache cache, ILogger logger, IOptions<RedisOptions> options)
        {
            Cache = cache;
            this.logger = logger;
            this.options = options.Value;
            ConnectionErrorTime = DateTimeOffset.MinValue;
            defaultOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(options.Value.CacheLifeTimeMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(options.Value.SlidingExpiration)
            };
        }

        public async Task<TResult?> GetValue<TResult>(string key)
        {
            if(!IsConnected) await TryConnect();

            if (IsConnected)
            {
                try
                {
                    TResult? value = JsonConvert.DeserializeObject<TResult>(await Cache.GetStringAsync(key) ?? "");
                    return value;
                }
                catch (Exception ex)
                {
                    logger.Error("Failed to connect to Redis");
                    logger.Error(ex.Message);
                    IsConnected = false;
                    ConnectionErrorTime = DateTimeOffset.Now;
                }
            }
            return default(TResult);
        }


        public async Task<bool> SetValue(string key, string value, DistributedCacheEntryOptions options)
        {
            if (!IsConnected) await TryConnect();

            if(IsConnected)
            {
                try
                {
                    await Cache.SetStringAsync(key, value, options);
                    return true;
                }
                catch (Exception ex)
                {
                    logger.Error("Failed to connect to Redis");
                    logger.Error(ex.Message);
                    IsConnected = false;
                    ConnectionErrorTime = DateTimeOffset.Now;
                }
            }
            return false;

        }

        public async Task<bool> SetValue(string key, string value)
        {
            return await SetValue(key, value, defaultOptions);
        }

        public async Task<bool> RemoveKey(string key)
        {
            if (!IsConnected) await TryConnect();

            if (IsConnected)
            {
                try
                {
                    await Cache.RemoveAsync(key);
                    return true;
                }
                catch (Exception ex)
                {
                    logger.Error("Failed to connect to Redis");
                    logger.Error(ex.Message);
                    IsConnected = false;
                    ConnectionErrorTime = DateTimeOffset.Now;
                }
            }
            return false;
        }

        public async Task<bool> TryConnect()
        {
            if(!options.IsUseRedis) { return false; }

            try
            {
                if (ValidateTimeout())
                {
                    logger.Debug($"Test Redis connection");
                    string? testValue = await Cache.GetStringAsync("test");
                    await Cache.SetStringAsync("test", "connected");
                    IsConnected = true;
                }
                else
                {
                    IsConnected = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Failed to connect to Redis");
                logger.Error(ex.Message);
                IsConnected = false;
                ConnectionErrorTime = DateTimeOffset.Now;
            }
            return IsConnected;
        }

        private bool ValidateTimeout()
        {
            if (this.options.ConnectionTimeout <= 0) return false;

            if(ConnectionErrorTime == DateTimeOffset.MinValue)
            {
                return true;
            }
            return DateTimeOffset.Now.AddMinutes(-1 * this.options.ConnectionTimeout) >= ConnectionErrorTime;
        }

    }
}
