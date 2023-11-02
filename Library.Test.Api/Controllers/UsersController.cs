using Library.Domain.Commands.User;
using Library.Domain.Dtos.User;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Implementataions;
using Library.Domain.Models.Interfaces;
using Library.Domain.Queries;
using Library.Domain.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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

        [HttpGet("{userId}")]
        public async Task<UserViewModel?> GetUser(Guid userId)
        {
            return await mediator.Send(new GetUserQuery { Id = userId });
        }

        [HttpGet]
        public async Task<List<UserViewModel>> GetUsers()
        {
            return await mediator.Send(new GetUsersQuery());
        }

        [HttpPatch]
        public async Task<bool> UpdateUser([FromBody] UserUpdateDto user)
        {
            return await mediator.Send(new UpdateUserCommand
            {
                User = user
            });
        }

        [HttpPost]
        public async Task<UserViewModel> Create([FromForm] CreateUserDto user)
        {
            UserModel newUser = new UserModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Age = user.Age,
                Description = user.Description,
                Email = user.Email,
                Password = user.Password
            };

            return await mediator.Send(new CreateUserCommand { User = newUser});
        }
    }
}