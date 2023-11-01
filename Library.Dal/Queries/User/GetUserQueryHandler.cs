using AutoMapper;
using Library.Dal.Models;
using Library.Dal.Options;
using Library.Domain.Dtos.User;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Interfaces;
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

namespace Library.Dal.Queries.User
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserViewModel?>
    {
        private readonly Database db;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ConnectionOptions options;
        public GetUserQueryHandler(Database db, IUnitOfWork unitOfWork, IOptions<ConnectionOptions> options, ILogger logger, IMapper mapper)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            this.options = options.Value;
        }

        public async Task<UserViewModel?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            logger.Debug("Get user by id");
            return mapper.Map<UserViewModel>( await db.GetCollection<UserEntity>(options.UserCollectionName).Find(x => x.Id == request.Id).FirstOrDefaultAsync());
        }
    }

    public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, AuthorViewModel?>
    {
        private readonly Database db;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ConnectionOptions options;
        public GetAuthorQueryHandler(Database db, IUnitOfWork unitOfWork, IOptions<ConnectionOptions> options, ILogger logger, IMapper mapper)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            this.options = options.Value;
        }

        public async Task<AuthorViewModel?> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            logger.Debug("Get user by id");
            return mapper.Map<AuthorViewModel>(await db.GetCollection<UserEntity>(options.UserCollectionName).Find(x => x.Id == request.Id).FirstOrDefaultAsync());
        }
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserViewModel>>
    {
        private readonly Database db;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ConnectionOptions options;
        public GetUsersQueryHandler(Database db, IUnitOfWork unitOfWork, IOptions<ConnectionOptions> options, ILogger logger, IMapper mapper)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            this.options = options.Value;
        }

        public async Task<List<UserViewModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            logger.Debug("Get all users");
            IEnumerable<UserEntity> users = db.GetCollection<UserEntity>(options.UserCollectionName).Find("{}").ToList();
            return mapper.Map<List<UserViewModel>>(users);
        }
    }
}
