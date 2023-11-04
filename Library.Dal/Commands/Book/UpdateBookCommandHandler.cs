using AutoMapper;
using Library.Common.Options;
using Library.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Commands.Book;
using Serilog;
using Library.Dal.Models;
using MongoDB.Driver;
using Library.Common.Exceptions;
using Newtonsoft.Json;
using Library.Domain.Constants;
using System.Net;

namespace Library.Dal.Commands.Book
{
    internal class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
    {
        private readonly Database db;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ICacheRepository cache;
        private readonly ConnectionOptions options;

        public UpdateBookCommandHandler(Database db, IOptions<ConnectionOptions> options, ILogger logger, IMapper mapper, ICacheRepository cache)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            this.cache = cache;
            this.options = options.Value;
        }
        public async Task<bool> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var bookCollection = db.GetCollection<BookEntity>(options.BookCollectionName);
            BookEntity bookEntity = await bookCollection.Find(x => x.Id == request.UpdatedBook.Id).FirstOrDefaultAsync();

            if(bookEntity == null) 
            {
                var msg = $"Book {request.UpdatedBook.Id} not found";
                logger.Error(msg);
                throw new ResponseResultException(System.Net.HttpStatusCode.NotFound, msg);
            }

            bookEntity.Author = request.UpdatedBook.Author;
            bookEntity.Title = request.UpdatedBook.Title;
            bookEntity.Publisher = request.UpdatedBook.Publisher;
            bookEntity.PiblishCity = request.UpdatedBook.PiblishCity;
            bookEntity.Discount = request.UpdatedBook.Discount;
            bookEntity.Price = request.UpdatedBook.Price;
            bookEntity.CoverType = request.UpdatedBook.CoverType;
            bookEntity.Description = request.UpdatedBook.Description;
            bookEntity.Genre = request.UpdatedBook.Genre;
            bookEntity.Pages = request.UpdatedBook.Pages;
            bookEntity.Year = request.UpdatedBook.Year;
            bookEntity.UpdateDate = DateTime.Now;


            var filter = Builders<BookEntity>.Filter.Eq(s=>s.Id, bookEntity.Id);

            var update = Builders<BookEntity>.Update
                .Set(s => s.Author, bookEntity.Author)
                .Set(s => s.Title, bookEntity.Title)
                .Set(s => s.Publisher, bookEntity.Publisher)
                .Set(s => s.PiblishCity, bookEntity.PiblishCity)
                .Set(s => s.Discount, bookEntity.Discount)
                .Set(s => s.Price, bookEntity.Price)
                .Set(s => s.CoverType, bookEntity.CoverType)
                .Set(s => s.Description, bookEntity.Description)
                .Set(s => s.Genre, bookEntity.Genre)
                .Set(s => s.Pages, bookEntity.Pages)
                .Set(s => s.Year, bookEntity.Year)
                .Set(s => s.UpdateDate, bookEntity.UpdateDate);

            try
            {
                var res = await bookCollection.UpdateOneAsync(filter, update);
                var bookStr = JsonConvert.SerializeObject(bookEntity);
                await cache.SetValue(CacheKeyPrefixes.BookKey(request.UpdatedBook.Id.ToString()), bookStr);
                return res.IsAcknowledged;
            }
            catch (Exception ex)
            {
                logger.Error($"Error while updating book {request.UpdatedBook.Id}");
                logger.Error(ex.Message);
                logger.Error(ex?.InnerException?.Message);
                throw new ResponseResultException(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}
