using AutoMapper;
using Library.Dal.Models;
using Library.Domain.Constants;
using Library.Domain.Dtos.User;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Queries.Book;
using Library.Domain.Queries.User;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Queries.User
{
    public class GetUserHistoryQueryHandler : IRequestHandler<GetUserHistoryQuery, UserHistoryViewModel>
    {
        private readonly ICacheRepository cache;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly Database db;
        private readonly IMediator mediator;

        public GetUserHistoryQueryHandler(ICacheRepository cache, ILogger logger, IMapper mapper, Database db, IMediator mediator)
        {
            this.cache = cache;
            this.logger = logger;
            this.mapper = mapper;
            this.db = db;
            this.mediator = mediator;
        }
        public async Task<UserHistoryViewModel> Handle(GetUserHistoryQuery request, CancellationToken cancellationToken)
        {
            HistoryEntity? historyEntity = await cache.GetValue<HistoryEntity>(CacheKeyPrefixes.HistoryKey(request.UserId.ToString()));
            if(historyEntity == null)
            {
                historyEntity = new HistoryEntity
                {
                    UserId = request.UserId
                };
            }
            UserHistoryViewModel userHistoryViewModel = new UserHistoryViewModel
            {
                UserId = request.UserId,
            };

            foreach (var book in historyEntity.Books)
            {
                var bookViewModel = await mediator.Send(new GetBookQuery { Id = book });
                if (bookViewModel == null) continue;
                userHistoryViewModel.Books.Add( bookViewModel);
            }

            return userHistoryViewModel;
        }
    }
}
