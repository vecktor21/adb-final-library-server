using AutoMapper;
using Library.Common.Exceptions;
using Library.Common.Options;
using Library.Domain.Commands.User;
using Library.Domain.Constants;
using Library.Domain.Dtos.User;
using Library.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Commands.User
{
    public class DeleteBookRequestHandler : IRequestHandler<DeleteUserCommand, bool>
    {

        private readonly Database db;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheRepository cache;
        private readonly ConnectionOptions options;

        public DeleteBookRequestHandler(Database db, IOptions<ConnectionOptions> options, ILogger logger, IMapper mapper, ICacheRepository cache)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            this.cache = cache;
            this.options = options.Value;
        }
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                UserViewModel? user = await cache.GetValue<UserViewModel>(CacheKeyPrefixes.UserKey(request.Id.ToString()));
                var filter = Builders<UserViewModel>.Filter.Eq(x => x.Id, request.Id);
                var collection = db.GetCollection<UserViewModel>(options.UserCollectionName);

                if (user == null)
                {
                    user = (await collection.FindAsync(filter)).FirstOrDefault();
                }

                if (user == null)
                {
                    throw new ResponseResultException(HttpStatusCode.NotFound, "User not found");
                }

                await collection.DeleteOneAsync(filter);

                return true;
            }
            catch (Exception ex)
            {
                logger.Error($"Error while deleting user {request.Id}");
                logger.Error(ex.Message);
                logger.Error(ex?.InnerException?.Message);
                throw new ResponseResultException(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }


    public class ClearUsersRequestHandler : IRequestHandler<ClearUsers, bool>
    {

        private readonly Database db;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheRepository cache;
        private readonly ConnectionOptions options;

        public ClearUsersRequestHandler(Database db, IOptions<ConnectionOptions> options, ILogger logger, IMapper mapper, ICacheRepository cache)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            this.cache = cache;
            this.options = options.Value;
        }
        public async Task<bool> Handle(ClearUsers request, CancellationToken cancellationToken)
        {
            var collection = db.GetCollection<UserViewModel>(options.UserCollectionName);
            await collection.DeleteManyAsync("{}");

            return true;
        }
    }
}
