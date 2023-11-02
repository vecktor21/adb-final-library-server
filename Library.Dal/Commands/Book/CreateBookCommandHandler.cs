using AutoMapper;
using Library.Common.Exceptions;
using Library.Dal.Models;
using Library.Common.Options;
using Library.Domain.Commands.Book;
using Library.Domain.Dtos.Book;
using Library.Domain.Models.Implementataions;
using Library.Domain.Models.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Commands.Book
{
    internal class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookViewModel>
    {
        private readonly Database db;
        private readonly IMediator mediator;
        private readonly ILogger logger;
        private readonly IOptions<ConnectionOptions> options;
        private readonly IMapper mapper;

        public CreateBookCommandHandler(Database db, IMediator mediator, ILogger logger, IOptions<ConnectionOptions> options, IMapper mapper)
        {
            this.db = db;
            this.mediator = mediator;
            this.logger = logger;
            this.options = options;
            this.mapper = mapper;
        }
        public async Task<BookViewModel> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            logger.Information("Create book");

            try
            {
                BookModel book = new BookModel
                {
                    AuthorId = request.Book.AuthorId,
                    Title = request.Book.Title,
                    Price = request.Book.Price,
                    Year = request.Book.Year,
                    Publisher = request.Book.Publisher,
                    CoverType = request.Book.CoverType,
                    PiblishCity = request.Book.PiblishCity,
                    Genre = request.Book.Genre,
                    Pages = request.Book.Pages,
                    Description = request.Book.Description,
                    Likes = new()
                };

                var bookEntiy = mapper.Map<BookEntity>(book);

                var collection = db.GetCollection<BookEntity>(options.Value.BookCollectionName);

                await collection.InsertOneAsync(bookEntiy);


                return mapper.Map< BookViewModel>( await (await collection.FindAsync<BookEntity>(x =>
                    x.Title == bookEntiy.Title
                    && x.AuthorId == bookEntiy.AuthorId
                    && x.CreateDate == bookEntiy.CreateDate
                )).FirstOrDefaultAsync());

            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.InnerException.Message);
                logger.Error(ex.StackTrace);
                throw new ResponseResultException(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
