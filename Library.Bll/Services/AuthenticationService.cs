using Library.Common.Exceptions;
using Library.Common.Options;
using Library.Domain.Dtos.User;
using Library.Domain.Interfaces.Services;
using Library.Domain.Models.Interfaces;
using Library.Domain.Queries.User;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library.Bll.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IOptions<JwtOptions> options;
        private readonly IMediator mediator;

        public AuthenticationService(IOptions<JwtOptions> options, IMediator mediator)
        {
            this.options = options;
            this.mediator = mediator;
        }
        public string GenerateToken(IUser user, int lifeTime)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.UTF8.GetBytes(options.Value.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = options.Value.Issuer,
                Audience = options.Value.Audience,
                Expires = DateTime.UtcNow.AddMinutes(lifeTime),
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("id", user.Id.ToString()), 
                    new Claim("role", user.Role)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }

        public async Task<string> Authorize(AuthorizeUserDto user)
        {
            string filter = String.Format("{{Email: \"{0}\"}}", user.Email);
            var foundUser = await mediator.Send(new GetUserByFilterQuery { Filter = filter });

            if(foundUser == null)
            {
                throw new ResponseResultException(System.Net.HttpStatusCode.NotFound, "User not found");
            }

            if (!foundUser.VerifyPassword(user.Password))
            {
                throw new ResponseResultException(System.Net.HttpStatusCode.BadRequest, "Wrong password");
            }

            return GenerateToken(foundUser, options.Value.Expires);
        }
    }
}
