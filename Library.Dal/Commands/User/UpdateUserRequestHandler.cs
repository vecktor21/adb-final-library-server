using Library.Common.Exceptions;
using Library.Dal.Models;
using Library.Common.Options;
using Library.Domain.Commands.User;
using Library.Domain.Models.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Commands.User
{
    public class UpdateUserRequestHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly Database db;
        private readonly ConnectionOptions options;
        private readonly ILogger logger;

        public UpdateUserRequestHandler(Database db, IOptions<ConnectionOptions> options, ILogger logger)
        {
            this.db = db;
            this.options = options.Value;
            this.logger = logger;
        }
        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            logger.Information("Updateing user");
            try
            {
                var updateDefinition = Builders<UserEntity>.Update
                .Set(x => x.Age, request.User.Age)
                .Set(x => x.Name, request.User.Name)
                .Set(x => x.Surname, request.User.Surname)
                .Set(x => x.PhoneNumber, request.User.PhoneNumber)
                .Set(x => x.Email, request.User.Email)
                .Set(x => x.Description, request.User.Description);
                var filter = Builders<UserEntity>.Filter
                .Eq(x => x.Id, request.User.Id);

                var updateResult = await db.GetCollection<UserEntity>(options.UserCollectionName).UpdateOneAsync(filter, updateDefinition);

                logger.Information($"Successfully updated user {request.User.Id}");
                return updateResult.IsAcknowledged;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.InnerException.Message);
                logger.Error(ex.StackTrace);
                throw new ResponseResultException(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
