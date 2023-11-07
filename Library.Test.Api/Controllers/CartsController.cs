using Library.Domain.Commands.Cart;
using Library.Domain.Dtos.Cart;
using Library.Domain.Queries.Cart;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly Serilog.ILogger logger;

        public CartsController(IMediator mediator, Serilog.ILogger logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<List<CartViewModel>> GetAllCarts()
        {
            return await mediator.Send(new GetAllCartsQuery());
        }
        [HttpGet("{userId}")]
        public async Task<CartViewModel> GetUserCart(Guid userId)
        {
            return await mediator.Send(new GetCartQuery { UserId = userId});
        }
        [HttpPost("{userId}")]
        public async Task<CartViewModel> CreateEmptyCart(Guid userId)
        {
            return await mediator.Send(new CreateEmptyCartCommand { UserId = userId });
        }
        [HttpPut("{userId}/book/{bookId}")]
        public async Task<bool> InsertBookCart(Guid userId, Guid bookId)
        {
            return await mediator.Send(new InsertBookToCartCommand { UserId = userId, BookId = bookId });
        }
        [HttpDelete("{userId}/book/{bookId}")]
        public async Task<bool> DecreaseBookCountFromCart(Guid userId, Guid bookId)
        {
            return await mediator.Send(new RemoveBookFromCartCommand { UserId = userId, BookId = bookId });
        }
    }
}
