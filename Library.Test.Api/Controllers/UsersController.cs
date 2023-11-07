using Library.Domain.Commands.User;
using Library.Domain.Dtos.Book;
using Library.Domain.Dtos.User;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Implementataions;
using Library.Domain.Models.Interfaces;
using Library.Domain.Queries;
using Library.Domain.Queries.Book;
using Library.Domain.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.CodeDom.Compiler;

namespace Library.Test.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;
        private readonly IMediator mediator;

        public UsersController(ILogger<UsersController> logger, IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        /// <summary>
        /// Получить пользователя по его Id
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <response code="200">Запрос обработан, пользователь получен</response>
        /// <response code="500">Внутренняя ошибка</response>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserViewModel?> GetUser(Guid userId)
        {
            return await mediator.Send(new GetUserQuery { Id = userId });
        }

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <response code="200">Запрос обработан, пользователи получены</response>
        /// <response code="500">Внутренняя ошибка</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<UserViewModel>> GetUsers()
        {
            return await mediator.Send(new GetUsersQuery());
        }

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        /// <param name="user">новая информация о пользователе</param>
        /// <param name="Age">Возраст пользователя</param>
        /// <response code="200">Запрос обработан, пользователь изменен</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка</response>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<bool> UpdateUser([FromBody] UserUpdateDto user)
        {
            return await mediator.Send(new UpdateUserCommand
            {
                User = user
            });
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="user">новая информация о пользователе</param>
        /// <response code="200">Запрос обработан, пользователь создан</response>
        /// <response code="400">Ошибка запроса. подробнее в ответе</response>
        /// <response code="500">Внутренняя ошибка</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserViewModel?> Create([FromForm] CreateUserDto user)
        {
            UserModel newUser = new UserModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Age = user.Age,
                Description = user.Description,
                Role = user.Role,
                Email = user.Email
            };
            newUser.Password = newUser.HashedPassword(user.Password);

            return await mediator.Send(new CreateUserCommand { User = newUser});
        }


        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="userId">новая информация о пользователе</param>
        /// <response code="200">Запрос обработан, пользователь удален</response>
        /// <response code="400">Ошибка запроса. подробнее в ответе</response>
        /// <response code="500">Внутренняя ошибка</response>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{userId}")]
        public async Task<bool> DeleteUser([FromForm] Guid userId) 
        { 
            return await mediator.Send(new DeleteUserCommand()
            {
                Id = userId
            });
        }

        /// <summary>
        /// Удаление Всех пользователей, доступно только пользователю с ролью ADMIN
        /// </summary>
        /// <param name="flag">дополнительный флаг для удаления. удалит только если будет true</param>
        /// <response code="200">Запрос обработан, пользователь удален</response>
        /// <response code="400">Ошибка запроса. подробнее в ответе</response>
        /// <response code="500">Внутренняя ошибка</response>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        [Authorize("ADMIN")]
        public async Task<bool> DeleteAllUsers([FromForm] bool flag)
        {
            if(flag) return await mediator.Send(new ClearUsers());
            return false;

        }


        /// <summary>
        /// Добавить книгу в историю просмотра
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="bookId">Id книги</param>
        /// <response code="200">Запрос обработан, книга добавлена в историю</response>
        /// <response code="500">Внутренняя ошибка</response>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{userId:Guid}/history/{bookId:Guid}")]
        public async Task<bool> AddBookToHistory(Guid userId, Guid bookId)
        {
            return await mediator.Send(new AddBookViewHistoryCommand { UserId = userId, BookId = bookId});
        }

        /// <summary>
        /// Получить историю просмотра пользователя
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <response code="200">Запрос обработан, история просмотра получена</response>
        /// <response code="500">Внутренняя ошибка</response>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{userId:Guid}/history")]
        public async Task<UserHistoryViewModel> GetUserViewHistory(Guid userId)
        {
            return await mediator.Send(new GetUserHistoryQuery { UserId = userId });
        }

        /// <summary>
        /// Получить рекомендации пользователя
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <response code="200">Запрос обработан, рекомендации получены</response>
        /// <response code="500">Внутренняя ошибка</response>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [HttpGet("{userId:Guid}/recomendations")]
        public async Task<List<BookViewModel>> GetUserRecomendations(Guid userId)
        {
            return await mediator.Send(new GetRecommendationsQuery { UserId = userId });
        }
    }
}