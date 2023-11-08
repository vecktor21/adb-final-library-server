using Library.Domain.Dtos;
using Library.Domain.Dtos.User;
using Library.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        string GenerateToken(IUser user, int lifeTime);
        Task<AuthorizationResponseDto> Authorize(AuthorizeUserDto user);
        Task<AuthorizationResponseDto> CheckAuth(string accessToken);
    }
}
