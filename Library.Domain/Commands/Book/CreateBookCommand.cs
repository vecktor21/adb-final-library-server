using Library.Domain.Dtos.Book;
using Library.Domain.Models.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Commands.Book
{
    public class CreateBookCommand : IRequest<BookViewModel>
    {
        public BookCreateDto Book { get; set; }
    }
}
