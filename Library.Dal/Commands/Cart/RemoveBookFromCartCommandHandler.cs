using AutoMapper;
using Library.Common.Exceptions;
using Library.Common.Options;
using Library.Dal.Models;
using Library.Domain.Commands.Cart;
using Library.Domain.Constants;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Interfaces;
using Library.Domain.Queries.Cart;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Commands.Cart
{
    public class RemoveBookFromCartCommandHandler : IRequestHandler<RemoveBookFromCartCommand, bool>
    {
        private readonly ILogger logger;
        private readonly Database db;
        private readonly ICacheRepository cache;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ConnectionOptions conectionOptions;
        private readonly RedisOptions cacheOptions;

        public RemoveBookFromCartCommandHandler(ILogger logger, Database db, ICacheRepository cache,
            IOptions<ConnectionOptions> conectionOptions, IOptions<RedisOptions> cacheOptions, IMediator mediator,
            IMapper mapper)
        {
            this.logger = logger;
            this.db = db;
            this.cache = cache;
            this.mediator = mediator;
            this.mapper = mapper;
            this.conectionOptions = conectionOptions.Value;
            this.cacheOptions = cacheOptions.Value;
        }
        public async Task<bool> Handle(RemoveBookFromCartCommand request, CancellationToken cancellationToken)
        {
            var cartCollection = db.GetCollection<CartEntity>(this.conectionOptions.CartCollectionName);
            CartEntity? cartEntity;

            CartEntity? cachedCart = await cache.GetValue<CartEntity>(CacheKeyPrefixes.CartKey(request.UserId.ToString()));
            if (cachedCart != null)
            {
                cartEntity = cachedCart;
            }
            else
            {
                cartEntity = await cartCollection.Find(x => x.UserId == request.UserId).FirstOrDefaultAsync();
            }

            if (cartEntity == null)
            {
                var msg = $"Cart for user {request.UserId} not found";
                logger.Error(msg);
                throw new ResponseResultException(HttpStatusCode.NotFound, msg);
            }

            var cartBook = cartEntity.Books.FirstOrDefault(x => x.BookId == request.BookId);

            if (cartBook == null)
            {
                var msg = $"Cart {request.UserId} does not contain book {request.BookId}";
                logger.Error(msg);
                throw new ResponseResultException(HttpStatusCode.BadRequest, msg);
            }

            if (cartBook.Count == 1)
            {
                cartEntity.Books.Remove(cartBook);
            }
            else
            {
                cartBook.Count--;
            }

            var filter = Builders<CartEntity>.Filter
                .Eq(s => s.UserId, request.UserId);
            var update = Builders<CartEntity>.Update
                .Set(s => s.Books, cartEntity.Books);

            try
            {
                var res = cartCollection.UpdateOne(filter, update);

                if(res.IsAcknowledged)
                {
                    var cartStr = JsonConvert.SerializeObject(cartEntity);
                    await cache.SetValue(CacheKeyPrefixes.CartKey(request.UserId.ToString()), cartStr);
                }

                return res.IsAcknowledged;
            }
            catch (Exception ex)
            {
                var mes = $"Error while removing book {request.BookId} from cart {request.UserId}";
                logger.Error(ex.Message);
                logger.Error(ex?.InnerException?.Message);
                throw new ResponseResultException(System.Net.HttpStatusCode.BadRequest, mes);
            }

        }
    }
}
