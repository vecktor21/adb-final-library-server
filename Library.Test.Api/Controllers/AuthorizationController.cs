using Library.Domain.Dtos;
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
        /// <param name="user">данные для авторизации: логин и пароль</param>
        /// <response code="200">Запрос обработан</response>
        /// <response code="500">Внутренняя ошибка</response>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<AuthorizationResponseDto> Authorize([FromBody]AuthorizeUserDto user)
        {
            return await auth.Authorize(user);
        }


        /// <summary>
        /// Авторизоваться (проверить валидность токена)
        /// </summary>
        /// <param name="accessToken">данные для авторизации: accessToken</param>
        /// <response code="200">Запрос обработан</response>
        /// <response code="401">Токен не действителен</response>
        /// <response code="500">Внутренняя ошибка</response>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<AuthorizationResponseDto> CheckAuth(string accessToken)
        {
            return await auth.CheckAuth(accessToken);
        }
    }
}
