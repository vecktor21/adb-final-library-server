using AutoMapper;
using Library.Common.Options;
using Library.Dal.Models;
using Library.Domain.Constants;
using Library.Domain.Dtos.Book;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Queries.Book;
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
    public class GetRecommendationsQueryHandler : IRequestHandler<GetRecommendationsQuery, List<BookViewModel>>
    {
        private readonly ILogger logger;
        private readonly Database db;
        private readonly ICacheRepository cache;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ConnectionOptions connOptions;

        public GetRecommendationsQueryHandler(ILogger logger, Database db, ICacheRepository cache, 
            IMapper mapper, IMediator mediator, IOptions<ConnectionOptions> connOptions)
        {
            this.logger = logger;
            this.db = db;
            this.cache = cache;
            this.mapper = mapper;
            this.mediator = mediator;
            this.connOptions = connOptions.Value;
        }

        public async Task<List<BookViewModel>> Handle(GetRecommendationsQuery request, CancellationToken cancellationToken)
        {
            var bookCol = db.GetCollection<BookEntity>(connOptions.BookCollectionName);
            List<BookViewModel> userBooks = new List<BookViewModel>();

            List<BookEntity> userLikedBooks = await bookCol.Find(x => x.Likes.Contains(request.UserId)).ToListAsync();

            userBooks.AddRange(mapper.Map<List<BookViewModel>>(userLikedBooks));

            HistoryEntity? userViewedBooks = await cache.GetValue<HistoryEntity>(CacheKeyPrefixes.HistoryKey(request.UserId.ToString()));

            if(userViewedBooks != null)
            {
                foreach (var bookId in userViewedBooks.Books)
                {
                    var bookViewModel = await mediator.Send(new GetBookQuery { Id = bookId });
                    if (bookViewModel == null) continue;
                    userBooks.Add(bookViewModel);
                }
            }

            var keyWords = userBooks.ToList().Select(x => x.Genre).ToList();
            keyWords.AddRange(userBooks.ToList().Select(x => x.Author).ToList());

            var recomendedBook = await bookCol.Find(x => keyWords.Contains(x.Genre) || keyWords.Contains(x.Author)).ToListAsync();

            return mapper.Map<List<BookViewModel>>(recomendedBook);
        }
    }
}
