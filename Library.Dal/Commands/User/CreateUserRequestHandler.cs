﻿using AutoMapper;
using Library.Common.Exceptions;
using Library.Dal.Models;
using Library.Common.Options;
using Library.Domain.Commands.User;
using Library.Domain.Dtos.User;
using Library.Domain.Interfaces.Repositories;
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

namespace Library.Dal.Commands.User
{
    internal class CreateUserRequestHandler : IRequestHandler<CreateUserCommand, UserViewModel?>
    {
        private readonly Database db;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ConnectionOptions options;

        public CreateUserRequestHandler(Database db, IOptions<ConnectionOptions> options, ILogger logger, IMapper mapper)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            this.options = options.Value;
        }
        public async Task<UserViewModel?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            logger.Information("Creating new user");
            var collection = db.GetCollection<UserEntity>(options.UserCollectionName);

            var filters = new List<FilterDefinition<UserEntity>>
            {
                Builders<UserEntity>.Filter.Eq(x=>x.Email,request.User.Email),
                Builders<UserEntity>.Filter.Eq(x=>x.PhoneNumber,request.User.PhoneNumber)
            };

            UserEntity foundUser;
            foreach (var filter in filters)
            {
                foundUser = (await collection.Find(filter).Limit(1).ToListAsync()).FirstOrDefault();
                if (foundUser != null)
                {
                    var errMsg = "This user already exists";
                    logger.Warning(errMsg);
                    throw new ResponseResultException(System.Net.HttpStatusCode.BadRequest, errMsg);
                }
            }

            var newUser = new UserEntity
            {
                Id = request.User.Id,
                Surname = request.User.Surname,
                Name = request.User.Name,
                Description = request.User.Description,
                Email = request.User.Email,
                Password = request.User.Password,
                Age = request.User.Age,
                PhoneNumber = request.User.PhoneNumber,
                RegisterDate = request.User.RegisterDate,
            };

            collection.InsertOne(newUser);

            var createdUser = (await collection.Find(filters[0]).Limit(1).ToListAsync()).FirstOrDefault(); ;
            var msg = $"Created new User {createdUser.Id}";
            logger.Information(msg);
            return mapper.Map<UserViewModel>( createdUser);
        }
    }
}

