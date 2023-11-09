using Library.Domain.Dtos.Book;
using Library.Domain.Models.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Queries.Book
{
    public class GetBookQuery : IRequest<BookViewModel?>
    {
        public Guid Id { get; set; }
    }
    public class GetAllBooksQuery : IRequest<List<BookViewModel>>
    {
    }

    public class GetBooksByFilterQuery : IRequest<List<BookViewModel>>
    {
        public string Filter { get; set; }
    }
}
