﻿using AutoMapper;
using Library.Dal.Models;
using Library.Dal.Options;
using Library.Domain.Dtos.Book;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Interfaces;
using Library.Domain.Queries.Book;
using Library.Domain.Queries.User;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Queries.Book
{
    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookViewModel?>
    {
        private readonly Database db;
        private readonly ILogger logger;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ConnectionOptions options;
        public GetBookQueryHandler(Database db, IOptions<ConnectionOptions> options, ILogger logger, IMediator mediator, IMapper mapper)
        {
            this.db = db;
            this.logger = logger;
            this.mediator = mediator;
            this.mapper = mapper;
            this.options = options.Value;
        }
        public async Task<BookViewModel?> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            logger.Debug("Get book by id");
            var book = mapper.Map<BookViewModel>( await db.GetCollection<BookEntity>(options.BookCollectionName).Find(x => x.Id == request.Id).FirstOrDefaultAsync());
            book.Author = await mediator.Send(new GetAuthorQuery { Id = book.AuthorId});

            return book;
        }
    }
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookViewModel>>
    {
        private readonly Database db;
        private readonly ILogger logger;
        private readonly ConnectionOptions options;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public GetAllBooksQueryHandler(Database db, IOptions<ConnectionOptions> options, ILogger logger, IMediator mediator, IMapper mapper)
        {
            this.db = db;
            this.logger = logger;
            this.options = options.Value;
            this.mediator = mediator;
            this.mapper = mapper;
        }
        public async Task<List<BookViewModel>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            logger.Debug("Get all books");
            IEnumerable<IBook> bookEntities = await db.GetCollection<BookEntity>(options.BookCollectionName).Find("{}").ToListAsync();

            var books = mapper.Map<IEnumerable<BookViewModel>>(bookEntities);

            foreach (var book in books)
            {
                book.Author = await mediator.Send(new GetAuthorQuery { Id = book.AuthorId });
            }
            return books.ToList();
        }
    }
}