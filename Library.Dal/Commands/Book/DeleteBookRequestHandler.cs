using AutoMapper;
using Library.Common.Exceptions;
using Library.Common.Options;
using Library.Domain.Commands.Book;
using Library.Domain.Commands.User;
using Library.Domain.Constants;
using Library.Domain.Dtos.Book;
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

namespace Library.Dal.Commands.Book
{
    public class DeleteBookRequestHandler : IRequestHandler<DeleteBookCommand, bool>
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
        public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            BookViewModel? book = await cache.GetValue<BookViewModel>(CacheKeyPrefixes.BookKey(request.Id.ToString()));
            var filter = Builders<BookViewModel>.Filter.Eq(x => x.Id, request.Id);
            var collection = db.GetCollection<BookViewModel>(options.BookCollectionName);

            if (book == null)
            {
                book = (await collection.FindAsync(filter)).FirstOrDefault();
            }

            if (book == null)
            {
                throw new ResponseResultException(HttpStatusCode.NotFound, "Book not found");
            }

            try
            {
                await collection.DeleteOneAsync(filter);
                await cache.RemoveKey(CacheKeyPrefixes.BookKey(request.Id.ToString()));

                return true;
            }
            catch (Exception ex)
            {
                logger.Error($"Error while deleting book {request.Id}");
                logger.Error(ex.Message);
                logger.Error(ex?.InnerException?.Message);
                throw new ResponseResultException(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }


    public class ClearBooksRequestHandler : IRequestHandler<ClearBooks, bool>
    {

        private readonly Database db;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheRepository cache;
        private readonly ConnectionOptions options;

        public ClearBooksRequestHandler(Database db, IOptions<ConnectionOptions> options, ILogger logger, IMapper mapper, ICacheRepository cache)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            this.cache = cache;
            this.options = options.Value;
        }
        public async Task<bool> Handle(ClearBooks request, CancellationToken cancellationToken)
        {
            try
            {
                var collection = db.GetCollection<BookViewModel>(options.BookCollectionName);
                await collection.DeleteManyAsync("{}");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error($"Error while deleting books");
                logger.Error(ex.Message);
                logger.Error(ex?.InnerException?.Message);
                throw new ResponseResultException(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
