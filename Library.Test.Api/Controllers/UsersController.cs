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
        /// �������� ������������ �� ��� Id
        /// </summary>
        /// <param name="userId">Id ������������</param>
        /// <response code="200">������ ���������, ������������ �������</response>
        /// <response code="500">���������� ������</response>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<UserViewModel?> GetUser(Guid userId)
        {
            return await mediator.Send(new GetUserQuery { Id = userId });
        }

        /// <summary>
        /// �������� ���� �������������
        /// </summary>
        /// <response code="200">������ ���������, ������������ ��������</response>
        /// <response code="500">���������� ������</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<UserViewModel>> GetUsers()
        {
            return await mediator.Send(new GetUsersQuery());
        }

        /// <summary>
        /// ���������� ������������
        /// </summary>
        /// <param name="user">����� ���������� � ������������</param>
        /// <param name="Age">������� ������������</param>
        /// <response code="200">������ ���������, ������������ �������</response>
        /// <response code="404">������������ �� ������</response>
        /// <response code="500">���������� ������</response>
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
        /// �������� ������������
        /// </summary>
        /// <param name="user">����� ���������� � ������������</param>
        /// <response code="200">������ ���������, ������������ ������</response>
        /// <response code="400">������ �������. ��������� � ������</response>
        /// <response code="500">���������� ������</response>
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
        /// �������� ������������
        /// </summary>
        /// <param name="userId">����� ���������� � ������������</param>
        /// <response code="200">������ ���������, ������������ ������</response>
        /// <response code="400">������ �������. ��������� � ������</response>
        /// <response code="500">���������� ������</response>
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
        /// �������� ���� �������������, �������� ������ ������������ � ����� ADMIN
        /// </summary>
        /// <param name="flag">�������������� ���� ��� ��������. ������ ������ ���� ����� true</param>
        /// <response code="200">������ ���������, ������������ ������</response>
        /// <response code="400">������ �������. ��������� � ������</response>
        /// <response code="500">���������� ������</response>
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
        /// �������� ����� � ������� ���������
        /// </summary>
        /// <param name="userId">Id ������������</param>
        /// <param name="bookId">Id �����</param>
        /// <response code="200">������ ���������, ����� ��������� � �������</response>
        /// <response code="500">���������� ������</response>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{userId:Guid}/history/{bookId:Guid}")]
        public async Task<bool> AddBookToHistory(Guid userId, Guid bookId)
        {
            return await mediator.Send(new AddBookViewHistoryCommand { UserId = userId, BookId = bookId});
        }

        /// <summary>
        /// �������� ������� ��������� ������������
        /// </summary>
        /// <param name="userId">Id ������������</param>
        /// <response code="200">������ ���������, ������� ��������� ��������</response>
        /// <response code="500">���������� ������</response>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{userId:Guid}/history")]
        public async Task<UserHistoryViewModel> GetUserViewHistory(Guid userId)
        {
            return await mediator.Send(new GetUserHistoryQuery { UserId = userId });
        }

        /// <summary>
        /// �������� ������������ ������������
        /// </summary>
        /// <param name="userId">Id ������������</param>
        /// <response code="200">������ ���������, ������������ ��������</response>
        /// <response code="500">���������� ������</response>
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