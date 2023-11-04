using Library.Domain.Dtos.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Commands.Book
{
    public class UpdateBookCommand : IRequest<bool>
    {
        public BookUpdateDto UpdatedBook { get; set; }

    }
}
