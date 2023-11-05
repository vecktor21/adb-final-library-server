using Library.Domain.Dtos.User;
using Library.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Library.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthenticationService auth;
        private readonly Serilog.ILogger logger;

        public AuthorizationController(IAuthenticationService auth, Serilog.ILogger logger)
        {
            this.auth = auth;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<string> Authorize([FromForm]AuthorizeUserDto authorizeUserDto)
        {
            return await auth.Authorize(authorizeUserDto);
        }
    }
}
