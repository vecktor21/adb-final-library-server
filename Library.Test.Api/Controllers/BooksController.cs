using Library.Domain.Commands.Book;
using Library.Domain.Dtos.Book;
using Library.Domain.Models.Implementataions;
using Library.Domain.Models.Interfaces;
using Library.Domain.Queries.Book;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("filter")]
        public async Task<List<BookViewModel>> GetBooksByFilter(string filter)
        {
            return await mediator.Send(new GetBooksByFilterQuery { Filter = filter});
        }

        [HttpPost]
        public async Task<BookViewModel> CreateBook([FromForm] BookCreateControllerDto book, [FromForm] IFormFileCollection images)
        {
            List<IFileModel> files = new List<IFileModel>();
            foreach (var file in HttpContext.Request.Form.Files)
            {
                if (!file.ContentType.Contains("image"))
                {
                    continue;
                }
                var fileModel = new FileModel
                {
                    ContentType = file.ContentType,
                    FileName = $"{Guid.NewGuid()}_{file.FileName}",
                    FileType = "image",
                    StaticFolder = "images"
                };

                MemoryStream ms = new();
                file.CopyTo(ms);
                fileModel.Content = ms.ToArray();

                files.Add(fileModel);

            }

            BookCreateDto newBook = new BookCreateDto
            {
                Author = book.Author,
                CoverType = book.CoverType,
                Description = book.Description,
                Genre = book.Genre,
                Images = files,
                Pages = book.Pages,
                PiblishCity = book.PiblishCity,
                Price = book.Price,
                Publisher = book.Publisher,
                Title = book.Title,
                Year = book.Year
            };

            return await mediator.Send(new CreateBookCommand { Book = newBook });
        }

        [HttpDelete("{bookId}")]
        public async Task<bool> DeleteBook(Guid bookId)
        {
            return await mediator.Send(new DeleteBookCommand()
            {
                Id = bookId
            });
        }

        [HttpDelete]
        [Authorize("ADMIN")]
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

        [HttpPost("{bookId:guid}/like/{userId:guid}")]
        public async Task<bool> LikeBook(Guid bookId, Guid userId)
        {
            return await mediator.Send(new LikeBookCommand { BookId = bookId, UserId = userId });
        }
    }
}
