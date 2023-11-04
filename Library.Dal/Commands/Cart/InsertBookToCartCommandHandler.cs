using AutoMapper;
using Library.Common.Options;
using Library.Dal.Models;
using Library.Domain.Commands.Cart;
using Library.Domain.Constants;
using Library.Domain.Dtos.Cart;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Queries.Cart;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Library.Common.Exceptions;

namespace Library.Dal.Commands.Cart
{
    public class InsertBookToCartCommandHandler : IRequestHandler<InsertBookToCartCommand, bool>
    {
        private readonly ILogger logger;
        private readonly Database db;
        private readonly ICacheRepository cache;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ConnectionOptions conectionOptions;
        private readonly RedisOptions cacheOptions;

        public InsertBookToCartCommandHandler(ILogger logger, Database db, ICacheRepository cache, 
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
        public async Task<bool> Handle(InsertBookToCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.Information($"Insert book {request.BookId} to cart {request.UserId}");
                CartEntity cart;

                CartEntity? cachedCart = await cache.GetValue<CartEntity>(CacheKeyPrefixes.CartKey(request.UserId.ToString()));
                if (cachedCart != null)
                {
                    cart = cachedCart;
                }
                else
                {
                    cart = mapper.Map<CartEntity>(await mediator.Send(new GetCartQuery { UserId = request.UserId }));
                }
                

                CartBookEntity? cartBook = cart.Books.FirstOrDefault(x => x.BookId == request.BookId);
                if(cartBook == null ) 
                { 
                    cartBook = new CartBookEntity { BookId = request.BookId, Count = 1 };
                }
                else
                {
                    cart.Books.Remove(cartBook);
                    cartBook.Count = cartBook.Count + 1;
                }

                cart.Books.Add(cartBook);

                var cartStr = JsonConvert.SerializeObject(cart);

                await cache.SetValue(CacheKeyPrefixes.CartKey(request.UserId.ToString()), cartStr);

                var col = db.GetCollection<CartEntity>(conectionOptions.CartCollectionName);

                var filter = Builders<CartEntity>.Filter.Eq(x => x.UserId, request.UserId);

                var update = Builders<CartEntity>.Update
                    .Set(x => x.Books, cart.Books);

                var res = await col.UpdateOneAsync(filter, update);

                logger.Information($"Inserted book {request.BookId} to cart {request.UserId}");

                return res.IsAcknowledged;
            }
            catch (Exception ex)
            {
                var mes = $"Error while inserting book {request.BookId} to cart {request.UserId}";
                logger.Error(ex.Message);
                logger.Error(ex?.InnerException?.Message);
                throw new ResponseResultException(System.Net.HttpStatusCode.BadRequest, mes);
            }

        }
    }
}
