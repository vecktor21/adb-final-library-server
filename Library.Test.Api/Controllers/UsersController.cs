using Library.Domain.Commands.User;
using Library.Domain.Dtos.User;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Implementataions;
using Library.Domain.Models.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Library.Test.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;
        private readonly IMediator mediator;

        public UsersController(ILogger<UsersController> logger, IUnitOfWork unitOfWork, IUserRepository userRepository, IMediator mediator)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
            this.mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IUser?> GetUser(Guid userId)
        {
            return await userRepository.FindUser(userId);
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
        public async Task<IUser> Create([FromForm] CreateUserDto user)
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