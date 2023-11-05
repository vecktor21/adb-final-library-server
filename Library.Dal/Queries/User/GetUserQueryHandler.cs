using AutoMapper;
using Library.Dal.Models;
using Library.Common.Options;
using Library.Domain.Dtos.User;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Interfaces;
using Library.Domain.Queries.User;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Constants;
using Library.Domain.Models.Implementataions;

namespace Library.Dal.Queries.User
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserViewModel?>
    {
        private readonly Database db;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheRepository cache;
        private readonly ConnectionOptions options;
        public GetUserQueryHandler(Database db, IOptions<ConnectionOptions> options, ILogger logger, IMapper mapper, ICacheRepository cache)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            this.cache = cache;
            this.options = options.Value;
        }

        public async Task<UserViewModel?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            logger.Debug("Get user by id");

            UserViewModel user;
            string key = CacheKeyPrefixes.UserKey(request.Id.ToString());
            UserViewModel? cachedUser = await cache.GetValue<UserViewModel>(key);
            

            if (cachedUser != null)
            {
                logger.Debug($"User {key} extracted from Cache");
                user = cachedUser;
            }
            else
            {
                user = mapper.Map<UserViewModel>(await db.GetCollection<UserEntity>(options.UserCollectionName).Find(x => x.Id == request.Id).FirstOrDefaultAsync());
                logger.Debug($"User {key} extracted from MongoDb");

                if (user!= null)
                {
                    var userStr = JsonConvert.SerializeObject(user);

                    if (await cache.SetValue(key, userStr))
                    {
                        logger.Debug($"User {key} written to Cache");
                    }
                }

            }

            return user;
        }
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserViewModel>>
    {
        private readonly Database db;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ConnectionOptions options;
        public GetUsersQueryHandler(Database db, IOptions<ConnectionOptions> options, ILogger logger, IMapper mapper)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            this.options = options.Value;
        }

        public async Task<List<UserViewModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            logger.Debug("Get all users");
            IEnumerable<UserEntity> users = db.GetCollection<UserEntity>(options.UserCollectionName).Find("{}").ToList();
            return mapper.Map<List<UserViewModel>>(users);
        }
    }


    public class GetUserEntityQueryHandler : IRequestHandler<GetUserByFilterQuery, UserModel?>
    {
        private readonly Database db;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheRepository cache;
        private readonly ConnectionOptions options;
        public GetUserEntityQueryHandler(Database db, IOptions<ConnectionOptions> options, ILogger logger, IMapper mapper, ICacheRepository cache)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            this.cache = cache;
            this.options = options.Value;
        }

        public async Task<UserModel?> Handle(GetUserByFilterQuery request, CancellationToken cancellationToken)
        {
            logger.Debug("Get user by id");

            UserModel user;

            user = mapper.Map<UserModel>(await db.GetCollection<UserEntity>(options.UserCollectionName).Find(request.Filter).FirstOrDefaultAsync());

            if (user != null)
            {
                var userStr = JsonConvert.SerializeObject(user);
                var key = user.Id.ToString();

                if (await cache.SetValue(CacheKeyPrefixes.UserKey(key), userStr))
                {
                    logger.Debug($"User {key} written to Cache");
                }
            }

            return user;
        }
    }
}
