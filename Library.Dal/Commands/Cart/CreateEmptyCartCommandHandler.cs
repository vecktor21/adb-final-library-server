using Serilog;
using AutoMapper;
using Library.Common.Options;
using Library.Dal.Models;
using Library.Domain.Commands.Cart;
using Library.Domain.Constants;
using Library.Domain.Dtos.Cart;
using Library.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Library.Common.Exceptions;

namespace Library.Dal.Commands.Cart
{
    public class CreateEmptyCartCommandHandler : IRequestHandler<CreateEmptyCartCommand, CartViewModel>
    {
        private readonly ILogger logger;
        private readonly Database db;
        private readonly IMapper mapper;
        private readonly ICacheRepository cache;
        private readonly RedisOptions redisOptions;
        private readonly ConnectionOptions options;

        public CreateEmptyCartCommandHandler(ILogger logger, Database db, IMapper mapper, IOptions<ConnectionOptions> options, 
            ICacheRepository cache, IOptions<RedisOptions> redisOptions)
        {
            this.logger = logger;
            this.db = db;
            this.mapper = mapper;
            this.cache = cache;
            this.redisOptions = redisOptions.Value;
            this.options = options.Value;
        }
        public async Task<CartViewModel> Handle(CreateEmptyCartCommand request, CancellationToken cancellationToken)
        {
            logger.Information($"Creating new cart for user {request.UserId}");
            var col = db.GetCollection<CartEntity>(options.CartCollectionName);

            if((await col.FindAsync(Builders<CartEntity>.Filter.Eq(x => x.UserId, request.UserId))).FirstOrDefault() != null)
            {
                var mes = $"User {request.UserId} already has cart";
                logger.Error(mes);
                throw new ResponseResultException(System.Net.HttpStatusCode.BadRequest, mes);
            }

            var newCart = new CartEntity
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId
            };

            var cartStr = JsonConvert.SerializeObject(newCart);
            await cache.SetValue(CacheKeyPrefixes.CartKey(request.UserId.ToString()), cartStr, new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(redisOptions.CartCacheLifeTime)
            });

            await col.InsertOneAsync(newCart);

            var cartViewModel = new CartViewModel
            {
                Id= newCart.Id,
                UserId= newCart.UserId,
                Books = new()
            };

            return cartViewModel;
        }
    }
}
