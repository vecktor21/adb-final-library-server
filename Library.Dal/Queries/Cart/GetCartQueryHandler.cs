using Amazon.Runtime.Internal;
using AutoMapper;
using Library.Common.Exceptions;
using Library.Common.Options;
using Library.Dal.Models;
using Library.Domain.Commands.Cart;
using Library.Domain.Constants;
using Library.Domain.Dtos.Book;
using Library.Domain.Dtos.Cart;
using Library.Domain.Dtos.User;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Implementataions;
using Library.Domain.Models.Interfaces;
using Library.Domain.Queries.Book;
using Library.Domain.Queries.Cart;
using Library.Domain.Queries.User;
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

namespace Library.Dal.Queries.Cart
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartViewModel>
    {
        private readonly ILogger logger;
        private readonly ICacheRepository cache;
        private readonly Database db;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ConnectionOptions options;

        public GetCartQueryHandler(ILogger logger, ICacheRepository cache, IOptions<ConnectionOptions> options, Database db, 
            IMapper mapper, IMediator mediator)
        {
            this.logger = logger;
            this.cache = cache;
            this.db = db;
            this.mapper = mapper;
            this.mediator = mediator;
            this.options = options.Value;
        }
        public async Task<CartViewModel> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            logger.Debug($"Get cartModel for user {request.UserId}");

            CartModel cartModel;
            CartEntity? cartEntity = await GetCartEntity(request.UserId);

            if(cartEntity == null) {
                return await mediator.Send(new CreateEmptyCartCommand { UserId = request.UserId });
            }

            cartModel = mapper.Map<CartModel>(cartEntity);

            foreach (var cartBook in cartEntity.Books)
            {
                BookModel bookModel = mapper.Map<BookModel>(await db.GetCollection<BookEntity>(options.BookCollectionName).Find(x => x.Id == cartBook.BookId).FirstOrDefaultAsync());
                if(bookModel == null) { continue; }
                cartModel.Books.Add(new CartBookModel { Book = bookModel, Count = cartBook.Count });
            }

            CartViewModel cartViewModel = mapper.Map<CartViewModel>(cartModel);

            return cartViewModel;

        }

        private async Task<CartEntity?> GetCartEntity(Guid userId)
        {
            string key = CacheKeyPrefixes.CartKey(userId.ToString());
            var cartEntity = await cache.GetValue<CartEntity>(key);


            if (cartEntity != null)
            {
                logger.Debug($"Cart {key} extracted from Cache");
            }
            else
            {
                cartEntity = await db.GetCollection<CartEntity>(options.CartCollectionName).Find(x => x.UserId == userId)
                    .FirstOrDefaultAsync();

                if(cartEntity == null)
                {
                    logger.Debug($"Cart {key} NotFound in Database");
                    return null;
                }

                logger.Debug($"Cart {key} extracted from MongoDb");
                var cartStr = JsonConvert.SerializeObject(cartEntity);

                if (await cache.SetValue(key, cartStr))
                {
                    logger.Debug($"Cart cart_{key} written to Cache");
                }
            }

            return cartEntity;
        }
    }


    public class GetAllCartsQueryHandler : IRequestHandler<GetAllCartsQuery, List<CartViewModel>>
    {
        private readonly ILogger logger;
        private readonly IMediator mediator;

        public GetAllCartsQueryHandler(ILogger logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }
        public async Task<List<CartViewModel>> Handle(GetAllCartsQuery request, CancellationToken cancellationToken)
        {
            var users = await mediator.Send(new GetUsersQuery());
            List<CartViewModel> carts = new List<CartViewModel>();
            foreach (var user in users)
            {
                carts.Add(await mediator.Send(new GetCartQuery { UserId = user.Id }));
            }
            return carts;
        }
    }
}
