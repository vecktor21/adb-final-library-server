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


        /// <summary>
        /// Авторизоваться (получить AccessToken)
        /// </summary>
        /// <param name="authorizeUserDto">данные для авторизации: логин и пароль</param>
        /// <response code="200">Запрос обработан</response>
        /// <response code="500">Внутренняя ошибка</response>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<string> Authorize([FromForm]AuthorizeUserDto authorizeUserDto)
        {
            return await auth.Authorize(authorizeUserDto);
        }
    }
}
