using AutoMapper;
using Library.Common.Options;
using Library.Dal.Models;
using Library.Domain.Commands.User;
using Library.Domain.Constants;
using Library.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Commands.User
{
    public class AddToViewHistoryCommandHandler : IRequestHandler<AddBookViewHistoryCommand, bool>
    {
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheRepository cache;
        private readonly RedisOptions cacheOpt;

        public AddToViewHistoryCommandHandler(ILogger logger, IMapper mapper, ICacheRepository cache, IOptions<RedisOptions> cacheOpt)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.cache = cache;
            this.cacheOpt = cacheOpt.Value;
        }
        public async Task<bool> Handle(AddBookViewHistoryCommand request, CancellationToken cancellationToken)
        {
            string key = CacheKeyPrefixes.HistoryKey(request.UserId.ToString());
            var userHistory = await cache.GetValue<HistoryEntity>(key);
            if (userHistory == null)
            {
                userHistory = new HistoryEntity
                {
                    UserId = request.UserId
                };
            }

            if (!userHistory.Books.Contains(request.BookId))
            {
                userHistory.Books.Add(request.BookId);
            }


            var histStr = JsonConvert.SerializeObject(userHistory);

            return await cache.SetValue(key, histStr, new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions { 
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(cacheOpt.UserHistoryLifeTime)
            });
        }
    }
}
