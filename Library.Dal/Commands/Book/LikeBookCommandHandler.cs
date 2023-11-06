using AutoMapper;
using Library.Common.Exceptions;
using Library.Common.Options;
using Library.Dal.Models;
using Library.Domain.Commands.Book;
using Library.Domain.Constants;
using Library.Domain.Interfaces.Repositories;
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

namespace Library.Dal.Commands.Book
{
    public class LikeBookCommandHandler : IRequestHandler<LikeBookCommand, bool>
    {
        private readonly ILogger logger;
        private readonly Database db;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly IOptions<RedisOptions> redisOptions1;
        private readonly IOptions<ConnectionOptions> connectionOptions1;
        private readonly ICacheRepository cache;
        private readonly RedisOptions redisOptions;
        private readonly ConnectionOptions connectionOptions;

        public LikeBookCommandHandler(ILogger logger, Database db, IMapper mapper, IMediator mediator, 
            IOptions<RedisOptions> redisOptions, IOptions<ConnectionOptions> connectionOptions, ICacheRepository cache)
        {
            this.logger = logger;
            this.db = db;
            this.mapper = mapper;
            this.mediator = mediator;
            redisOptions1 = redisOptions;
            connectionOptions1 = connectionOptions;
            this.cache = cache;
            this.redisOptions = redisOptions.Value;
            this.connectionOptions = connectionOptions.Value;
        }
        public async Task<bool> Handle(LikeBookCommand request, CancellationToken cancellationToken)
        {
            logger.Debug($"Like book {request.BookId} by user {request.UserId}");

            var bookCol = db.GetCollection<BookEntity>(connectionOptions.BookCollectionName);

            string key = CacheKeyPrefixes.BookKey(request.BookId.ToString());
            var cachedBook = await cache.GetValue<BookEntity>(key);

            BookEntity book;
            if(cachedBook != null)
            {
                book = cachedBook;
            }
            else
            {
                book = await bookCol.Find(x => x.Id == request.BookId).FirstOrDefaultAsync();
            }

            if(book == null)
            {
                string msg = $"Book {request.BookId} not found";
                logger.Error(msg);
                throw new ResponseResultException(System.Net.HttpStatusCode.NotFound, msg);
            }

            Like(book, request.UserId);

            var bookStr = JsonConvert.SerializeObject(book);

            if(await cache.SetValue(key, bookStr))
            {
                logger.Information($"Book {request.BookId} written to cache");
            }

            var filter = Builders<BookEntity>.Filter.Eq(x => x.Id, request.BookId);
            var update = Builders<BookEntity>.Update
                .Set(x => x.Likes, book.Likes);

            await bookCol.UpdateOneAsync(filter, update);

            return true;
        }



        private bool Like(BookEntity book, Guid userId)
        {
            if (book.Likes.Contains(userId))
            {
                book.Likes.Remove(userId);
                return true;
            }
            else
            {
                book.Likes.Add(userId);
                return true;
            }
        }
    }
}
