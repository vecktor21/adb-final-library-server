using Library.Domain.Commands.Book;
using Library.Domain.Commands.Book;
using Library.Domain.Dtos.Book;
using Library.Domain.Models.Interfaces;
using Library.Domain.Queries.Book;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Library.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IMediator mediator;

        public BooksController(ILogger logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        [HttpGet("{bookId}")]
        public async Task<BookViewModel?> GetBook(Guid bookId)
        {
            return await mediator.Send(new GetBookQuery { Id = bookId });
        }


        [HttpGet]
        public async Task<List<BookViewModel>> GetBooks()
        {
            return await mediator.Send(new GetAllBooksQuery());
        }


        [HttpPost]
        public async Task<BookViewModel> CreateBook([FromForm]BookCreateDto book)
        {
            return await mediator.Send(new CreateBookCommand { Book = book});
        }

        [HttpDelete("{bookId}")]
        public async Task<bool> DeleteBook([FromForm] Guid bookId)
        {
            return await mediator.Send(new DeleteBookCommand()
            {
                Id = bookId
            });
        }

        [HttpDelete]
        public async Task<bool> DeleteAllBooks([FromForm] bool flag)
        {
            if (flag) return await mediator.Send(new ClearBooks());
            return false;

        }
        [HttpPatch]
        public async Task<bool> UpdateBook([FromForm] BookUpdateDto updatedBook)
        {
            return await mediator.Send(new UpdateBookCommand
            {
                UpdatedBook = updatedBook
            });
        }

    }
}
